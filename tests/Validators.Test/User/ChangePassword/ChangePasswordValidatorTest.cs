using CommonTestUtilities.Requests;
using EStudy.Application.UseCases.User.ChangePassword;
using FluentAssertions;

namespace Validators.Test.User.ChangePassword;

public class ChangePasswordValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new ChangePasswordValidator();

        var request = RequestChangePasswordJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Error_Password_Invalid(int passwordLength)
    {
        var validator = new ChangePasswordValidator();

        var request = RequestChangePasswordJsonBuilder.Build(passwordLength);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals("Password must be at least 6 characters long"));
    }

    [Fact]
    public void Error_Password_Empty()
    {
        var validator = new ChangePasswordValidator();

        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals("Password is required"));
    } 
}