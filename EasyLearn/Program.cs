using EasyLearn.Data;
using EasyLearn.GateWays.Email;
using EasyLearn.GateWays.FileManager;
using EasyLearn.Repositories.Implementations;
using EasyLearn.Repositories.Interfaces;
using EasyLearn.Services.Implementations;
using EasyLearn.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EasyLearn;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var configuration = builder.Configuration.GetConnectionString("EasyLearnDbConnectionString");

        builder.Services.AddDbContext<EasyLearnDbContext>(options => options.UseMySql(configuration, ServerVersion.AutoDetect(configuration)));
        builder.Services
       .AddOptions<CompanyInfoOption>()
       .BindConfiguration("CompanyInfo");

        builder.Services
            .AddOptions<PaystackOptions>()
            .BindConfiguration("Paystack");

        builder.Services
            .AddOptions<SendinblueOptions>()
            .BindConfiguration("SendinblueAPIKey");

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserService, UserService>();

        //builder.Services.AddScoped<IAdminRepository, AdminRepository>();
        //builder.Services.AddScoped<IAdminService, AdminService>();

        builder.Services.AddScoped<IFileManagerService, FileManagerService>();
        builder.Services.AddScoped<ISendInBlueEmailService, SendInBlueEmailService>();
        builder.Services.AddScoped<CompanyInfoOption>();
        builder.Services.AddScoped<SendinblueOptions>();
        builder.Services.AddScoped<PaystackOptions>();



        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();
        EasyLearnDbInitializer.Seed(app);

        app.Run();
    }
}
