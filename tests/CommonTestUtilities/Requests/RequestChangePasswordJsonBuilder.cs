using Bogus;
using EStudy.Communication.Requests.ChangePassword;

namespace CommonTestUtilities.Requests;

public static class RequestChangePasswordJsonBuilder
{
    public static RequestChangePasswordJson Build(int passwordLength = 10)
    {
        return new Faker<RequestChangePasswordJson>()
            .RuleFor(u => u.Password, (f) => f.Internet.Password())
            .RuleFor(u => u.NewPassword, (f) => f.Internet.Password(passwordLength));
    }
}