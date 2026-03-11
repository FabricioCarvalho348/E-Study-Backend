using EStudy.Application.Services.AutoMapper;
using EStudy.Application.UseCases.Login.DoLogin;
using EStudy.Application.UseCases.Token;
using EStudy.Application.UseCases.User.ChangePassword;
using EStudy.Application.UseCases.User.Delete.Delete;
using EStudy.Application.UseCases.User.Delete.Request;
using EStudy.Application.UseCases.User.Profile;
using EStudy.Application.UseCases.User.Register;
using EStudy.Application.UseCases.User.Update;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sqids;

namespace EStudy.Application;

public static class ApplicationModule
{
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddAutoMapper(services);
        AddIdEncoder(services, configuration);
        AddUseCases(services);
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddScoped(option => new AutoMapper.MapperConfiguration(autoMapperOptions =>
        {
            var sqIds = option.GetService<SqidsEncoder<long>>()!;

            autoMapperOptions.AddProfile(new AutoMapping(sqIds));
        }, option.GetService<ILoggerFactory>()).CreateMapper());
    }

    private static void AddIdEncoder(IServiceCollection services, IConfiguration configuration)
    {
        var alphabet = configuration.GetValue<string>("Settings:IdCryptographyAlphabet");
        if (string.IsNullOrWhiteSpace(alphabet))
            throw new InvalidOperationException("Sqids alphabet is not configured. Set Settings:IdCryptographyAlphabet in environment variables, appsettings, or user secrets.");
        var sqIds = new SqidsEncoder<long>(new()
        {
            MinLength = 3,
            Alphabet = alphabet
        });

        services.AddSingleton(sqIds);
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
        services.AddScoped<IRequestDeleteUserUseCase, RequestDeleteUserUseCase>();
        services.AddScoped<IDeleteUserAccountUseCase, DeleteUserAccountUseCase>();
        services.AddScoped<IUseRefreshTokenUseCase, UseRefreshTokenUseCase>();
    }
}