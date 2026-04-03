using CommonTestUtilities.Requests;
using EStudy.Application.UseCases.User.Register;
using FluentAssertions;

namespace Validators.Test.User.Register;

public class RegisterUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_Name_Empty()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals("O nome é obrigatório."));
    }

    [Fact]
    public void Error_Email_Empty()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals("O email é obrigatório."));
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = "email.com";

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals("O email informado é inválido."));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Error_Password_Invalid(int passwordLength)
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build(passwordLength);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals("Password must be at least 6 characters long"));
    }

    [Fact]
    public void Error_Password_Empty()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();
        request.Password = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals("Password is required"));
    }
}