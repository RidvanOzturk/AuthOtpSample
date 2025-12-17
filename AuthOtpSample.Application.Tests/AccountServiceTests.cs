//using AuthOtpSample.Application.Abstractions.Common;
//using AuthOtpSample.Application.Abstractions.Notifications;
//using AuthOtpSample.Application.DTOs;
//using AuthOtpSample.Application.Services.Implementations;
//using AuthOtpSample.Domain.Entities;
//using AuthOtpSample.Infrastructure.Database;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.InMemory;
//using Moq;

//namespace AuthOtpSample.Application.Tests;

//public sealed class AccountServiceTests
//{
//    private static ApplicationDbContext CreateDbContext()
//    {
//        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())

//            .Options;

//        var currentUserMock = new Mock<ICurrentUser>();
//        currentUserMock.SetupGet(x => x.UserId).Returns(1);

//        return new ApplicationDbContext(options, currentUserMock.Object);
//    }

//    [Fact]
//    public async Task RegisterAsync_Should_Create_User_And_Send_ConfirmationEmail()
//    {
//        await using var dbContext = CreateDbContext();

//        var emailSenderMock = new Mock<IEmailSender>();
//        var smsSenderMock = new Mock<ISmsSender>();

//        var sut = new AccountService(
//            dbContext,
//            emailSenderMock.Object,
//            smsSenderMock.Object);

//        var request = new RegisterDto("test@example.com", "P@ssw0rd!");

//        await sut.RegisterAsync(request, CancellationToken.None);

//        var users = await dbContext.Users.ToListAsync();
//        Assert.Single(users);
//        var user = users.First();
//        Assert.Equal("test@example.com", user.Email);
//        Assert.False(string.IsNullOrWhiteSpace(user.HashPassword));
//        Assert.False(user.IsActive);

//        // Assert: OTP üretildi mi?
//        var otps = await dbContext.Otps.ToListAsync();
//        Assert.Single(otps);
//        var otp = otps.First();
//        Assert.Equal(user.Id, otp.UserId);
//        Assert.Equal(OtpType.Confirmation, otp.Type);
//        Assert.True(otp.ExpirationDate > DateTime.UtcNow);

//        emailSenderMock.Verify(x =>
//                x.SendAsync(
//                    "test@example.com",
//                    It.Is<string>(s => s.Contains("Confirm", StringComparison.OrdinalIgnoreCase)),
//                    It.Is<string>(body => body.Contains("Your OTP code", StringComparison.OrdinalIgnoreCase)),
//                    It.IsAny<CancellationToken>()),
//            Times.Once);
//    }
//}