using BierBockBackend.Auth;

namespace BierBock.Tests
{
    public class UnitTestHashing
    {
        [Theory]
        [InlineData("test1234_")]
        public void GivenPwd_WhenGeneratingHash_ShouldVerify(string pwd)
        {
            // Arrange
            var hash = PasswordHashing.HashPassword(pwd);

            // Act
            var result = PasswordHashing.VerifyPassword(pwd, hash.Hash, hash.Salt);

            // Assert
            Assert.True(result);
        }
    }
}