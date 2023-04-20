using BierBockBackend.Auth;

namespace BierBock.Tests;

public class UnitTestSendEmail
{
    [Fact]
    public void TestSendMail()
    {
      var x=  EmailSender.SendEmailAsync("i20030@hb.dhbw-stuttgart-de", "Hi", "www.heise.de").Result;
      Assert.True(x.IsSuccessStatusCode);
    }
}