namespace BierBockBackend.Auth;

public interface IEmailSender
{
    Task SendConfirmationMail(string toEmail, string token, string username);
}