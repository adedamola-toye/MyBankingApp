using System;
using System.Collections.Generic;
using System.Linq;
using BankingApp.Models;
using BankingApp.Services;
using BankingApp.Interfaces;
using Moq;
using Xunit;

namespace BankingApp.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IDataStore<string, User>> _mockDataStore;
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Dictionary<string, User> _userDict;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockDataStore = new Mock<IDataStore<string, User>>();
            _mockAuthService = new Mock<IAuthService>();
            _userDict = new Dictionary<string, User>();
            _mockDataStore.Setup(ds => ds.Load()).Returns(_userDict);
            _userService = new UserService(_mockDataStore.Object, _mockAuthService.Object);
        }

        [Fact]
        public void Register_SuccessfulRegistration_ReturnsSuccess()
        {
            _mockAuthService.Setup(a => a.HashPassword("password")).Returns("hashed");
            var result = _userService.Register("user1", "password");
            Assert.True(result.Success);
            Assert.Equal("Registration successful", result.Message);
            Assert.Single(_userDict.Values);
        }

        [Fact]
        public void Register_UsernameTaken_ReturnsError()
        {
            var existingUser = new User("user1", "hash");
            _userDict.Add(existingUser.UserId, existingUser);
            var result = _userService.Register("user1", "password");
            Assert.False(result.Success);
            Assert.Equal("Username already exists.", result.Message);
        }

        [Fact]
        public void Register_InvalidInput_ReturnsError()
        {
            var result = _userService.Register("", "");
            Assert.False(result.Success);
            Assert.Equal("Username and password are required.", result.Message);
        }

        [Fact]
        public void Login_Successful_ReturnsUser()
        {
            var user = new User("user1", "hash");
            _userDict.Add(user.UserId, user);
            _mockAuthService.Setup(a => a.VerifyPassword("password", "hash")).Returns(true);
            var result = _userService.Login("user1", "password");
            Assert.True(result.Success);
            Assert.Equal("Login successful.", result.Message);
            Assert.NotNull(result.User);
            Assert.Equal("user1", result.User.Username);
        }

        [Fact]
        public void Login_WrongPassword_ReturnsError()
        {
            var user = new User("user1", "hash");
            _userDict.Add(user.UserId, user);
            _mockAuthService.Setup(a => a.VerifyPassword("wrong", "hash")).Returns(false);
            var result = _userService.Login("user1", "wrong");
            Assert.False(result.Success);
            Assert.Equal("Incorrect password.", result.Message);
            Assert.Null(result.User);
        }

        [Fact]
        public void Login_UserNotFound_ReturnsError()
        {
            var result = _userService.Login("nouser", "password");
            Assert.False(result.Success);
            Assert.Equal("User does not exist.", result.Message);
            Assert.Null(result.User);
        }

        [Fact]
        public void Login_InvalidInput_ReturnsError()
        {
            var result = _userService.Login("", "");
            Assert.False(result.Success);
            Assert.Equal("Username and password are required.", result.Message);
            Assert.Null(result.User);
        }

        [Fact]
        public void GetUser_ExistingUser_ReturnsUser()
        {
            var user = new User("user1", "hash");
            _userDict.Add(user.UserId, user);
            var result = _userService.GetUser("user1");
            Assert.NotNull(result);
            Assert.Equal("user1", result.Username);
        }

        [Fact]
        public void GetUser_NonExistingUser_ReturnsNull()
        {
            var result = _userService.GetUser("nouser");
            Assert.Null(result);
        }

        [Fact]
        public void GetAllUsers_ReturnsAllUsers()
        {
            var user1 = new User("user1", "hash1");
            var user2 = new User("user2", "hash2");
            _userDict.Add(user1.UserId, user1);
            _userDict.Add(user2.UserId, user2);
            var result = _userService.GetAllUsers();
            Assert.Equal(2, result.Count);
            Assert.Contains(result, u => u.Username == "user1");
            Assert.Contains(result, u => u.Username == "user2");
        }
    }
}
