using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BTest.Infrastructure.Email;
using BTest.Infrastructure.Identity.DTO;
using BTest.Infrastructure.Identity.Entities;
using BTest.Infrastructure.Identity.Enum;
using BTest.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


namespace BTest.Infrastructure.Identity;

public interface IAccountService
{
  Task<BaseResponse<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request);
  Task<BaseResponse<string>> RegisterAsync(RegisterRequest request, string uri);
  Task<BaseResponse<string>> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest);
  Task<BaseResponse<AuthenticationResponse>> RefreshTokenAsync(RefreshTokenRequest request);

}
public class AccountService : IAccountService
{
  private string loginProvider = "BTest";
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly RoleManager<ApplicationRole> _roleManager;
  private readonly SignInManager<ApplicationUser> _signInManager;
  private readonly JWTSettings _jwtSettings;
  private readonly IEmailService _emailService;
  public AccountService(UserManager<ApplicationUser> userManager,
      RoleManager<ApplicationRole> roleManager,
      IOptions<JWTSettings> jwtSettings,
      SignInManager<ApplicationUser> signInManager,
      IEmailService emailService)
  {
    _userManager = userManager;
    _roleManager = roleManager;
    _signInManager = signInManager;
    _jwtSettings = jwtSettings.Value;
    _emailService = emailService;
  }
  public async Task<BaseResponse<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
  {
    ApplicationUser user = await _userManager.FindByEmailAsync(request.Email.Trim());
    if (user == null)
    {
      throw new ApiException($"You are not registered with '{request.Email}'.")
      {
        StatusCode = (int)HttpStatusCode.BadRequest
      };
    }
    if (!user.EmailConfirmed)
    {
      throw new ApiException($"Account Not Confirmed for '{request.Email}'.")
      {
        StatusCode = (int)HttpStatusCode.BadRequest        
      };
    }

    SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, false, lockoutOnFailure: false);
    if (!signInResult.Succeeded)
    {
      throw new ApiException($"Invalid Credentials for '{request.Email}'.")
      {
        StatusCode = (int)HttpStatusCode.BadRequest
      };
    }


    JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
    AuthenticationResponse response = new AuthenticationResponse();
    response.Id = user.Id.ToString();
    response.AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    response.Email = user.Email;
    response.UserName = user.UserName;
    IList<string> rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
    response.Roles = rolesList.ToList();
    response.IsVerified = user.EmailConfirmed;
    response.RefreshToken = await GenerateRefreshToken(user);
    return new BaseResponse<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
  }

  public async Task<BaseResponse<string>> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest)
  {
    var user = await _userManager.FindByEmailAsync(confirmEmailRequest.Email);
    if (user.EmailConfirmed) throw new ApiException("This email confirmed already") { StatusCode = (int)HttpStatusCode.BadRequest };
    confirmEmailRequest.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(confirmEmailRequest.Code));
    var result = await _userManager.ConfirmEmailAsync(user, confirmEmailRequest.Code);
    if (result.Succeeded)
    {
      return new BaseResponse<string>(user.Email, message: $"Account Confirmed for {user.Email}. You can login with your password !");
    }
    else
    {
      throw new ApiException($"An error occured while confirming {user.Email}.") { StatusCode = (int)HttpStatusCode.InternalServerError };
    }
  }

  public async Task ForgotPasswordAsync(ForgotPasswordRequest request, string uri)
  {
    var user = await _userManager.FindByEmailAsync(request.Email);
    if (user == null) return;

    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
    var route = "api/account/reset-password/";
    var enpointUri = new Uri(string.Concat($"{uri}/", route));
    var emailRequest = new EmailRequest()
    {
      Body = $"You have to send a request to the '{enpointUri}' service with reset token - {code}",
      To = request.Email,
      Subject = "Reset Password",
    };
    await _emailService.SendAsync(emailRequest);
  }

  public async Task<BaseResponse<string>> LogoutAsync(string userEmail)
  {
    ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);
    if (user != null)
    {
      await _userManager.RemoveAuthenticationTokenAsync(user, loginProvider, "RefreshToken");
    }
    await _signInManager.SignOutAsync();

    return new BaseResponse<string>(userEmail, message: $"Logout.");
  }

  public async Task<BaseResponse<AuthenticationResponse>> RefreshTokenAsync(RefreshTokenRequest request)
  {
    var principal =GetPrincipalFromExpiredToken(request.AccessToken);
    ApplicationUser user = await _userManager.FindByEmailAsync(principal.FindFirstValue(ClaimTypes.Email));

    if (user == null)
    {
      throw new ApiException($"Not registered.") { StatusCode = (int)HttpStatusCode.BadRequest };
    }
    if (!user.EmailConfirmed)
    {
      throw new ApiException($"Not Confirmed.") { StatusCode = (int)HttpStatusCode.BadRequest };
    }

    JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
    AuthenticationResponse response = new AuthenticationResponse();
    response.Id = user.Id.ToString();
    response.AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    response.Email = user.Email;
    response.UserName = user.UserName;
    IList<string> rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
    response.Roles = rolesList.ToList();
    response.IsVerified = user.EmailConfirmed;
    response.RefreshToken = await GenerateRefreshToken(user);

    await _signInManager.SignInAsync(user, false);
    return new BaseResponse<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
  }

  public async Task<BaseResponse<string>> RegisterAsync(RegisterRequest request, string uri)
  {
    var findUser = await _userManager.FindByEmailAsync(request.Email);
    if (findUser != null)
    {
      throw new ApiException($"Email {request.Email} is already registered.") { StatusCode = (int)HttpStatusCode.BadRequest };
    }
    ApplicationUser newUser = new ApplicationUser
    {
      Email = request.Email,
      UserName = request.Email
    };
    var result = await _userManager.CreateAsync(newUser, request.Password);
    if (result.Succeeded)
    {
      await _userManager.AddToRoleAsync(newUser, Roles.Basic.ToString());
      var verificationUri = await SendVerificationEmail(newUser, uri);

      return new BaseResponse<string>(verificationUri, message: $"User Registered. Please confirm your account by visiting this URL {verificationUri}");
    }
    else
    {
      throw new ApiException($"{string.Join(Environment.NewLine, result.Errors.Select(t => t.Description))}") { StatusCode = (int)HttpStatusCode.InternalServerError };
    }
  }

  public async Task<BaseResponse<string>> ResetPasswordAsync(ResetPasswordRequest request)
  {
    var user = await _userManager.FindByEmailAsync(request.Email);
    if (user == null) throw new ApiException($"You are not registered with '{request.Email}'.");

    var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
    if (result.Succeeded)
    {
      return new BaseResponse<string>(request.Email, message: $"Password Resetted.");
    }
    else
    {
      throw new ApiException($"Error occured while reseting the password. Please try again.");
    }
  }

  public async Task<List<ApplicationUser>> GetUsers()
  {
    //return await _userManager.Users.ToListAsync();

    return await _userManager.Users.ToListAsync(); //lazzyloading
  }

  private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
  {
    var userClaims = await _userManager.GetClaimsAsync(user);
    var roles = await _userManager.GetRolesAsync(user);

    var roleClaims = new List<Claim>();

    for (int i = 0; i < roles.Count; i++)
    {
      roleClaims.Add(new Claim("roles", roles[i]));
    }

    var claims = new[]
    {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id.ToString())
            }
    .Union(userClaims)
    .Union(roleClaims);

    var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
    var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

    var jwtSecurityToken = new JwtSecurityToken(
        issuer: _jwtSettings.Issuer,
        audience: _jwtSettings.Audience,
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
        signingCredentials: signingCredentials);
    return jwtSecurityToken;
  }

  private async Task<string> GenerateRefreshToken(ApplicationUser user)
  {
    string newRefreshToken = string.Empty;
    try
    {
      await _userManager.RemoveAuthenticationTokenAsync(user, loginProvider, "RefreshToken");
      newRefreshToken = await _userManager.GenerateUserTokenAsync(user, loginProvider, "RefreshToken");
      IdentityResult result = await _userManager.SetAuthenticationTokenAsync(user, loginProvider, "RefreshToken", newRefreshToken);
      if (!result.Succeeded)
      {
        throw new ApiException($"An error occured while set refreshtoken.") { StatusCode = (int)HttpStatusCode.InternalServerError };
      }
    }
    catch (Exception ex)
    {

    }
    return newRefreshToken;
  }

  private string RandomTokenString()
  {
    using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
    var randomBytes = new byte[40];
    rngCryptoServiceProvider.GetBytes(randomBytes);
    // convert random bytes to hex string
    return BitConverter.ToString(randomBytes).Replace("-", "");
  }

  private async Task<string> SendVerificationEmail(ApplicationUser newUser, string uri)
  {
    var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
    var route = "login/";
    var _enpointUri = new Uri(string.Concat($"{uri}/", route));
    var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "email", newUser.Email.ToString());
    verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);

    //await _emailService.SendAsync(new EmailRequest()
    //{
    //  From = "from@btest.com",
    //  To = newUser.Email,
    //  Body = $"Please confirm your account by visiting this URL {verificationUri}",
    //  Subject = "Confirm Registration"
    //});

    return verificationUri;
  }

  private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
  {
    var tokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
      ValidateAudience = false, 
      ValidateIssuer = false,
      RoleClaimType = ClaimTypes.Role,
      ClockSkew = TimeSpan.Zero,
      ValidateLifetime = false 
    };
    var tokenHandler = new JwtSecurityTokenHandler();
    SecurityToken securityToken;
    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
    var jwtSecurityToken = securityToken as JwtSecurityToken;
    if (jwtSecurityToken == null || 
      !jwtSecurityToken.Header.Alg.Equals(
        SecurityAlgorithms.HmacSha256, 
        StringComparison.InvariantCultureIgnoreCase))
      throw new SecurityTokenException("Invalid token");
    return principal;
  }
}
