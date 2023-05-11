#region

using System.Net;
using System.Net.Mail;

#endregion

namespace BierBockBackend.Auth;

public class EmailSender : IEmailSender
{
    public Task SendConfirmationMail(string toEmail, string token, string username)
    {
        var MyServer = new SmtpClient();
        MyServer.Host = "smtp.sendgrid.net";
        MyServer.Port = 25;

        //Server Credentials
        var NC = new NetworkCredential
        {
            UserName = "apikey",
            Password = "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"
        };

        MyServer.Credentials = NC;

        var from = new MailAddress("i20030@hb.dhbw-stuttgart.de", "Bierbock");

        var receiver = new MailAddress(toEmail, "User");

        var msg = new MailMessage(from, receiver);
        msg.Subject = "Bierbock E-Mail Adresse bestätigen";

        var url = $"https://www.beerbock.de/security/confirmEmail?emailToken={token}&username={username}";

        msg.Body =
            $"<h1>Willkommen bei Bierbock</h1>\r\n<h2>Bitte bestätigen Sie ihre E-Mail Adresse</h2>\r\n<a href=\"{url}\">Hier bestätigen</a>";
        msg.IsBodyHtml = true;

        MyServer.Send(msg);
        return Task.CompletedTask;
    }
}