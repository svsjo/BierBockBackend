#region

using BierBockBackend.Auth;

#endregion

namespace BierBock.Tests;

public class UnitTestSendEmail
{
    [Theory]
    [InlineData("jona.schwab@gmx.de", "123", "jona")]
    public void GivenMail_WhenSending_ShouldComplete(string toEmail, string token, string username)
    {
        // Act
        var completed = new EmailSender().SendConfirmationMail(toEmail, token, username).IsCompleted;

        // Assert
        Assert.True(completed);
    }
}