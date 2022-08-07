using BTest.Infrastructure.Identity;
using BTest.Infrastructure.Identity.DTO;
using BTest.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BTest.SPA.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  private readonly IAccountService _accountService;
  // logging etc.
  public AuthController(IAccountService accountService)
  {
    _accountService = accountService;
  }

  [AllowAnonymous]
  [HttpPost("login")]
  public async Task<BaseResponse<AuthenticationResponse>> GetTokenAsync([FromBody] AuthenticationRequest authenticationRequest)
  {
    return await _accountService.AuthenticateAsync(authenticationRequest);
  }

  [AllowAnonymous]
  [HttpPost("register")]
  public async Task<BaseResponse<string>> RegisterAsync([FromBody] RegisterRequest registerRequest)
  {
    return await _accountService.RegisterAsync(registerRequest, $"{Request.Scheme}://{Request.Host.Value}");
  }

  [AllowAnonymous]
  [HttpPost("confirm-email")]
  public async Task<BaseResponse<string>> ConfirmEmailAsync([FromBody] ConfirmEmailRequest confirmEmailRequest)
  {
    return await _accountService.ConfirmEmailAsync(confirmEmailRequest);   
  }

  [AllowAnonymous]
  [HttpPost("refresh-token")]
  public async Task<BaseResponse<AuthenticationResponse>> RefreshTokenAsync([FromBody] RefreshTokenRequest confirmEmailRequest)
  {
    return await _accountService.RefreshTokenAsync(confirmEmailRequest);
  }
}
