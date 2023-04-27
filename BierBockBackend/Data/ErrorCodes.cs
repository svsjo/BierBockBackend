// ReSharper disable InconsistentNaming
namespace BierBockBackend.Data;

public class ErrorCodes
{
    public string Message { get; }

    public ErrorCodes(string message)
    {
        Message = message;
    }

    public static ErrorCodes invalid_username = new(nameof(invalid_username));
    public static ErrorCodes username_taken = new(nameof(username_taken));
    public static ErrorCodes beer_not_found = new(nameof(beer_not_found));
    public static ErrorCodes mail_not_confirmed = new(nameof(mail_not_confirmed));
    public static ErrorCodes mail_taken = new(nameof(mail_taken));
    public static ErrorCodes invalid_firstname_format = new (nameof(invalid_firstname_format));
    public static ErrorCodes invalid_surname_format = new(nameof(invalid_surname_format));
    public static ErrorCodes invalid_password_format = new(nameof(invalid_password_format));
    public static ErrorCodes invalid_email_format = new(nameof(invalid_email_format));
    public static ErrorCodes invalid_birthdate_format = new(nameof(invalid_birthdate_format));
    public static ErrorCodes user_not_found = new (nameof(user_not_found));
    public static ErrorCodes invalid_password = new(nameof(invalid_password));
}