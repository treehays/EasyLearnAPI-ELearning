using EasyLearn.Authentication;
using EasyLearn.Data;
using EasyLearn.GateWays.Email;
using EasyLearn.GateWays.FileManager;
using EasyLearn.Repositories.Implementations;
using EasyLearn.Repositories.Interfaces;
using EasyLearn.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EasyLearn.Services.Implementations;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        return services.AddDbContext<EasyLearnDbContext>(x => x.UseSqlServer(connectionString));
    }
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services.AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IFileManagerService, FileManagerService>()
            .AddScoped<ISendInBlueEmailService, SendInBlueEmailService>()
            .AddScoped<ITokenService, TokenService>()
            .AddScoped<CompanyInfoOption>()
            .AddScoped<SendinblueOptions>()
            .AddScoped<PaystackOptions>();
    }
}
