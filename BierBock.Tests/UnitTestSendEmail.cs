using BierBockBackend.Auth;

namespace BierBock.Tests;

public class UnitTestSendEmail
{
    [Fact]
    public void TestSendMail()
    {
      var x=  new EmailSender().SendConfirmationMail("jona.schwab@gmx.de",  "123","jona").IsCompleted;
      Assert.True(x);
    }
}