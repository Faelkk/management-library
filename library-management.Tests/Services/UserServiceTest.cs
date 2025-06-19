using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.Services;
using LibraryManagement.Dto;
using LibraryManagement.UserRepository;
using System;
using LibraryManagement.Models;

public class UserServiceTest
{
    private readonly Mock<IUserRepository> userRepositoryMock;
    private readonly Mock<IEmailService> emailServiceMock;
    private readonly Mock<ITokenGenerator> tokenGeneratorMock;
    private readonly Mock<IPasswordService> passwordServiceMock;
    private readonly UserService userService;

    public UserServiceTest()
    {
        userRepositoryMock = new Mock<IUserRepository>();
        emailServiceMock = new Mock<IEmailService>();
        tokenGeneratorMock = new Mock<ITokenGenerator>();
        passwordServiceMock = new Mock<IPasswordService>();

        userService = new UserService(
            userRepositoryMock.Object,
            emailServiceMock.Object,
            tokenGeneratorMock.Object,
            passwordServiceMock.Object
        );
    }

    [Fact]
    public void GetAll_ReturnsUserResponseDtos()
    {
        var users = new List<UserResponseDto>
        {
            new UserResponseDto { Id = 1, Name = "Alice", Email = "alice@mail.com", Role = "User", PhoneNumber = "123" },
            new UserResponseDto { Id = 2, Name = "Bob", Email = "bob@mail.com", Role = "Admin", PhoneNumber = "456" }
        };

        userRepositoryMock.Setup(r => r.GetAll()).Returns(users);

        var result = userService.GetAll();

        Assert.Collection(result,
            u =>
            {
                Assert.Equal(1, u.Id);
                Assert.Equal("Alice", u.Name);
            },
            u =>
            {
                Assert.Equal(2, u.Id);
                Assert.Equal("Bob", u.Name);
            }
        );
    }

    [Fact]
    public void GetById_ExistingUser_ReturnsUserResponseDto()
    {
        var userResponseDto = new UserResponseDto { Id = 1, Name = "Alice", Email = "alice@mail.com", Role = "User", PhoneNumber = "123" };
        userRepositoryMock.Setup(r => r.GetById(1)).Returns(userResponseDto);

        var result = userService.GetById(1);

        Assert.Equal(1, result.Id);
        Assert.Equal("Alice", result.Name);
        Assert.Equal("alice@mail.com", result.Email);
    }

    [Fact]
    public void GetById_NonExistingUser_ThrowsException()
    {
        userRepositoryMock.Setup(r => r.GetById(99)).Returns((UserResponseDto)null);

        Assert.Throws<Exception>(() => userService.GetById(99));
    }

    [Fact]
    public void Create_ValidUser_ReturnsTokenAndSendsEmail()
    {
        var userInsertDto = new UserInsertDto { Name = "Alice", Email = "alice@mail.com", Password = "pass" };
        var createdUserResponseDto = new UserResponseDto { Id = 1, Name = "Alice", Email = "alice@mail.com", Role = "User", PhoneNumber = "123" };

        userRepositoryMock.Setup(r => r.Create(userInsertDto)).Returns(createdUserResponseDto);
        tokenGeneratorMock.Setup(t => t.Generate(It.IsAny<UserResponseDto>())).Returns("token123");

        var result = userService.Create(userInsertDto);

        Assert.Equal("token123", result.Token);
        emailServiceMock.Verify(e => e.Send(It.Is<Message>(m => m.MailTo == "alice@mail.com")), Times.Once);
    }

    [Fact]
    public void Login_ValidCredentials_ReturnsTokenAndSendsEmail()
    {
        var userLoginDto = new UserLoginDto { Email = "alice@mail.com", Password = "pass" };
        var loggedUserResponseDto = new UserResponseDto { Id = 1, Name = "Alice", Email = "alice@mail.com", Role = "User", PhoneNumber = "123" };

        userRepositoryMock.Setup(r => r.Login(userLoginDto)).Returns(loggedUserResponseDto);
        tokenGeneratorMock.Setup(t => t.Generate(loggedUserResponseDto)).Returns("token456");

        var result = userService.Login(userLoginDto, "TestAgent");

        Assert.Equal("token456", result.Token);
        emailServiceMock.Verify(e => e.Send(It.Is<Message>(m => m.MailTo == "alice@mail.com")), Times.Once);
    }

    [Fact]
    public async Task RecoverPassword_ReturnsResponse()
    {
        var recoveryDto = new UserRecoveryPasswordDto { email = "alice@mail.com" };
        var responseDto = new UserResponseMessageDto { Message = "Recovery sent" };

        passwordServiceMock.Setup(p => p.ProcessPasswordRecovery(recoveryDto)).ReturnsAsync(responseDto);

        var result = await userService.RecoverPassword(recoveryDto);

        Assert.Equal("Recovery sent", result.Message);
    }

    [Fact]
    public async Task ResetPassword_ReturnsResponse()
    {
        var resetDto = new UserResetPasswordDto { password = "newpass" };
        var responseDto = new UserResponseMessageDto { Message = "Password reset" };

        passwordServiceMock.Setup(p => p.ProcessPasswordReset(resetDto, "token")).ReturnsAsync(responseDto);

        var result = await userService.ResetPassword(resetDto, "token");

        Assert.Equal("Password reset", result.Message);
    }

    [Fact]
    public async Task Remove_CallsRepositoryRemove()
    {
        userRepositoryMock.Setup(r => r.Remove(1)).Returns(Task.CompletedTask);

        await userService.Remove(1);

        userRepositoryMock.Verify(r => r.Remove(1), Times.Once);
    }
}