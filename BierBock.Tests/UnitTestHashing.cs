using BierBockBackend.Auth;

namespace BierBock.Tests
{
    public class UnitTestHashing
    {
        [Fact]
        public void TestHashing()
        {
            var pwd = "test1234_";
            var hash = PasswordHashing.HashPassword(pwd);
            Assert.True(PasswordHashing.VerifyPassword(pwd, hash.Hash, hash.Salt));
        }
    }
}