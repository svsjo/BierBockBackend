using SendGrid;
using SendGrid.Helpers.Mail;

namespace BierBockBackend.Auth;

public class EmailSender 
{
    public static async Task<Response> SendEmailAsync(string toEmail, string subject, string token)
    {
        var client = new SendGridClient("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");
        var msg = new SendGridMessage()
        {
            From = new EmailAddress("Joe@contoso.com", "Password Recovery"),
            Subject = subject,
            HtmlContent = $"<a href=\"{token}\">Confirm E-Mail<a/>"
        };
        msg.AddTo(new EmailAddress(toEmail));

        msg.SetClickTracking(false, false);
        var response = await client.SendEmailAsync(msg);
        return response;
    }
}