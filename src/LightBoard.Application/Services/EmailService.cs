using LightBoard.Application.Abstractions.Arguments;
using LightBoard.Application.Abstractions.Options;
using LightBoard.Application.Abstractions.Services;
using LightBoard.Shared.Api;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace LightBoard.Application.Services;

public class EmailService : IEmailService
{
    private readonly MailingOptions _options;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<MailingOptions> options, ILogger<EmailService> logger)
    {
        _logger = logger;
        _options = options.Value;
    }
    public async void Send(SendMailArgs args)
    {
        // create message
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_options.FromName, _options.Email));
        email.To.Add(MailboxAddress.Parse(args.To));
        email.Subject = args.Subject;
        email.Body = new TextPart(TextFormat.Html) { Text = args.Html };

        // send email
        using var smtp = new SmtpClient();
        try
        {
            await smtp.ConnectAsync(_options.Host, _options.Port);
        }
        catch (Exception e)
        {
            _logger.LogError("Connection to smtp failed.");
            throw;
        }
        await smtp.AuthenticateAsync(_options.Email, _options.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}