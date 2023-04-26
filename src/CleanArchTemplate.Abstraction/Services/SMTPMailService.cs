﻿using CleanArchTemplate.Application.Common.Configurations;
using CleanArchTemplate.Application.Common.Interfaces.Services;
using CleanArchTemplate.Shared.Requests.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace CleanArchTemplate.Abstraction.Services;

public class SMTPMailService : IMailService
{
    private readonly MailConfiguration _config;
    private readonly ILogger<SMTPMailService> _logger;

    public SMTPMailService(IOptionsMonitor<MailConfiguration> config, ILogger<SMTPMailService> logger)
    {
        _config = config.CurrentValue;
        _logger = logger;
    }

    public async Task SendAsync(MailRequest request)
    {
        try
        {
            var email = new MimeMessage
            {
                Sender = new MailboxAddress(_config.DisplayName, request.From ?? _config.From),
                Subject = request.Subject,
                Body = new BodyBuilder
                {
                    HtmlBody = request.Body
                }.ToMessageBody()
            };
            email.To.Add(MailboxAddress.Parse(request.To));
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config.Host, _config.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config.UserName, _config.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }
    }
}