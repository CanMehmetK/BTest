// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using BTest.Infrastructure.Interfaces;
using BTest.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace BTest.Mvc.Areas.Identity.Pages.Account
{
  [AllowAnonymous]
  public class ResendEmailConfirmationModel : PageModel
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMailService _emailSender;

    public ResendEmailConfirmationModel(UserManager<ApplicationUser> userManager, IMailService emailSender)
    {
      _userManager = userManager;
      _emailSender = emailSender;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
      /// <summary>
      ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
      ///     directly from your code. This API may change or be removed in future releases.
      /// </summary>
      [Required]
      [EmailAddress]
      public string Email { get; set; }
    }

    public void OnGet()
    {
    }
    public bool DisplayConfirmAccountLink { get; set; }
    public string EmailConfirmationUrl { get; set; }
    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
      {
        return Page();
      }

      var user = await _userManager.FindByEmailAsync(Input.Email);
      if (user == null)
      {
        ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");

        return Page();
      }

      var userId = await _userManager.GetUserIdAsync(user);
      var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
      code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
      EmailConfirmationUrl = Url.Page(
         "/Account/ConfirmEmail",
         pageHandler: null,
         values: new { userId = userId, code = code },
         protocol: Request.Scheme);
      DisplayConfirmAccountLink = true;
      if (!DisplayConfirmAccountLink)
        await _emailSender.SendAsync(new EMail(
                         to: new List<string>() { Input.Email },
                         "Confirm your email",
                         $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(EmailConfirmationUrl)}'>clicking here</a>.")
                   );
      ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
      return Page();
    }
  }
}
