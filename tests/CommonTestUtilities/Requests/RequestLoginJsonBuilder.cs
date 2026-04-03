using Bogus;
using EStudy.Communication.Requests.DoLogin;

namespace CommonTestUtilities.Requests;

public class RequestLoginJsonBuilder
{
    public static RequestLoginJson Build()
    {
        return new Faker<RequestLoginJson>()
            .RuleFor(u => u.Email, (f) => f.Internet.Email())
            .RuleFor(u => u.Password, (f) => f.Internet.Password());
    }
}