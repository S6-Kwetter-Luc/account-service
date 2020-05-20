﻿using System;
using System.Threading.Tasks;
using account_service.Controllers;
using account_service.Entities;
using account_service.Exceptions;
using account_service.Helpers;
using account_service.Models;
using account_service.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace account_service_test.Controllers
{
    public class UsersControllerTest
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private UsersController _controller;
        private Hasher _hasher;

        public UsersControllerTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _hasher = new Hasher();
        }

        [Fact]
        public async Task Register_Successful()
        {
            var userService = new Mock<IUserService>();

            _controller = new UsersController(userService.Object);

            var result = _controller.Register(new RegisterModel()
                {Name = "test1", Email = "test@test.nl", Password = "testtest"});

            _testOutputHelper.WriteLine(result.ToString());
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Register_Badresult()
        {
            var userService = new Mock<IUserService>();
            _controller = new UsersController(userService.Object);

            userService.Setup(p => p.Create("test1", "test@test.nl","test1", "testtest"))
                .Throws<AlreadyInUseException>();

            var result = _controller.Register(new RegisterModel()
                {Name = "test1", Email = "test@test.nl", Username="test1", Password = "testtest"});

            // _testOutputHelper.WriteLine(result.ToString());
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Authenticate_Successful()
        {
            var userService = new Mock<IUserService>();
            _controller = new UsersController(userService.Object);

            var salt = _hasher.CreateSalt();
            var hashedPassword = await _hasher.HashPassword("testtest", salt);
            var guid = new Guid();

            userService.Setup(p => p.Authenticate("test@test.nl", "testtest"))
                .Returns(async () => new User()
                {
                    Id = guid,
                    Email = "test@test.nl",
                    Name = "test1",
                    Salt = salt,
                    Password = hashedPassword,
                    OauthIssuer = "none",
                    Token = "",
                    OauthSubject = ""
                });

            var result = await _controller.Authenticate(new AuthenticateModel()
            {
                Email = "test@test.nl",
                Password = "testtest"
            });

            _testOutputHelper.WriteLine(result.ToString());

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Authenticate_Badresult()
        {
            var userService = new Mock<IUserService>();
            _controller = new UsersController(userService.Object);

            var result = await _controller.Authenticate(new AuthenticateModel()
            {
                Email = "test@test.nl",
                Password = "testtest"
            });

            _testOutputHelper.WriteLine(result.ToString());

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}