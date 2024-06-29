using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Serilog;

namespace KursovaWork.Infrastructure.Services.Helpers.Static;

/// <summary>
/// Class for sending emails.
/// </summary>
public static class EmailSenderHelper
{
    /// <summary>
    /// Sends an email.
    /// </summary>
    /// <param name="mail">Recipient's email address.</param>
    /// <param name="subject">Subject of the email.</param>
    /// <param name="message">Body of the email.</param>
    public static void SendEmail(string mail, string subject, string message)
    {
        Log.Information("Entering the email sending method");

        var email = new MimeMessage();

        email.From.Add(new MailboxAddress("VAG Dealer", "baryaroman@ukr.net"));
        email.To.Add(new MailboxAddress("Dear customer", mail));
        email.Subject = subject;
        email.Body = new TextPart("html") { Text = message };

        Log.Information("Email data set for sending");

        using (var smtp = new SmtpClient())
        {
            smtp.Connect("smtp.ukr.net", 465, SecureSocketOptions.SslOnConnect);
            smtp.Authenticate("baryaroman@ukr.net", "a94DSYBDuxMIT8l4");
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        Log.Information("Email sent successfully");
    }
}