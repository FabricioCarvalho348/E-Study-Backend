using AutoMapper;
using CommonTestUtilities.IdEncryption;
using EStudy.Application.Services.AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace CommonTestUtilities.Mapper;

public class MapperBuilder
{
    public static IMapper Build()
    {
        var idEncrypter = IdEncrypterBuilder.Build();

        var mapper = new MapperConfiguration(options =>
        {
            options.AddProfile(new AutoMapping(idEncrypter));
        }, NullLoggerFactory.Instance).CreateMapper();

        return mapper;
    }
}