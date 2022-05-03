using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using MoneyLaunderingBatch.Configurations;
using MoneyLaunderingBatch.Contracts;

namespace MoneyLaunderingBatch.Services;
public class EmailSender : IEmailSender
{
    private readonly SmtpClient _client;
    private readonly MoneyLaunderingEmailOptions _emailOptions;

    public EmailSender(IOptions<MoneyLaunderingEmailOptions> emailOptions)
    {
        _emailOptions = emailOptions.Value;
        _client = new SmtpClient(host: _emailOptions.Host, port: _emailOptions.Port)
        {
            Credentials = new NetworkCredential(userName: _emailOptions.Username, password: _emailOptions.Password),
            EnableSsl = true
        };
    }
    public void SendEmail(string receiver, string subject, string body)
    {
        Console.WriteLine($"Sending email too: {receiver} - {subject}");

        try
        {
            _client.Send(from: _emailOptions.Sender, receiver, subject, body);

        }
        catch (Exception e)
        {
            Console.WriteLine("Email could not be sent");
            Console.WriteLine(e);
            throw;
        }

        Console.WriteLine("Sent");
    }
}