using BankingApp.Services;
using BankingApp.Helpers;
using Xunit;

namespace BankingApp.Tests
{
    public class AuthServiceTests
    {
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _authService = new AuthService();
        }

        [Fact]
        public void HashPassword_ReturnsHashedPassword()
        {
            string password = "myPassword123";
            string hash = _authService.HashPassword(password);
            Assert.False(string.IsNullOrWhiteSpace(hash));
            Assert.NotEqual(password, hash);
        }

        [Fact]
        public void VerifyPassword_CorrectPassword_ReturnsTrue()
        {
            string password = "myPassword123";
            string hash = _authService.HashPassword(password);
            bool result = _authService.VerifyPassword(password, hash);
            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_WrongPassword_ReturnsFalse()
        {
            string password = "myPassword123";
            string hash = _authService.HashPassword(password);
            bool result = _authService.VerifyPassword("wrongPassword", hash);
            Assert.False(result);
        }

        [Fact]
        public void HashPassword_EmptyPassword_ReturnsHash()
        {
            string password = string.Empty;
            string hash = _authService.HashPassword(password);
            Assert.False(string.IsNullOrWhiteSpace(hash));
        }
    }
}
