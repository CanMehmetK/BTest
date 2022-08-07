﻿using BTest.Infrastructure.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BTest.Infrastructure.Email;

public interface IEmailService
{
  Task SendAsync(EmailRequest request);
}

public class EmailService : IEmailService
{
  private readonly MailSettings _mailSettings;

  public EmailService(IOptions<MailSettings> mailSettings)
  {
    _mailSettings = mailSettings.Value;
  }

  public async Task SendAsync(EmailRequest request)
  {
    try
    {
      var email = new MimeMessage();
      email.Sender = MailboxAddress.Parse(request.From ?? _mailSettings.EmailFrom);
      email.To.Add(MailboxAddress.Parse(request.To));
      email.Subject = request.Subject;
      var builder = new BodyBuilder();
      builder.HtmlBody = request.Body;
      email.Body = builder.ToMessageBody();
      using var smtp = new SmtpClient();
      smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
      smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
      await smtp.SendAsync(email);
      smtp.Disconnect(true);
    }
    catch (Exception ex)
    {
      // !
      throw new ApiException(ex.Message);
    }
  }
}
