using System.Text;
using EasyLearn.Data;
using EasyLearn.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace EasyLearn;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionstring = builder.Configuration.GetConnectionString("EasyLearnDbConnectionString2");
        builder.Services.AddDatabase(connectionstring);


        builder.Services
            .AddOptions<CompanyInfoOption>()
            .BindConfiguration("CompanyInfo");

        builder.Services
            .AddOptions<PaystackOptions>()
            .BindConfiguration("Paystack");

        builder.Services
            .AddOptions<SendinblueOptions>()
            .BindConfiguration("SendinblueAPIKey");

        builder.Services.AddRepositories();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddControllers();
        //addung custome origi nCORS
        //builder.Services.AddCors(options =>
        //{
        //    options.AddPolicy("AllowOrigin",
        //        builder => builder.WithOrigins("http://127.0.0.1:5500")
        //            .AllowAnyHeader()
        //            .AllowAnyMethod());
        //});

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "EasyLearn API",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Email = "Treehays90@gmail.com",
                        Name = "S3m1c0l0n",
                    },
                    Description = "This is just an educational Application",

                });

                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "EasyLearnAuthIssuer",
                    ValidAudience = "EasyLearnAuthAudience",
                    IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("!SomethingSecret!")
            ),
                };
            });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.EnableTryItOutByDefault();
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();


        // Configure middleware
        //app.UseCors("AllowOrigin");//to specify the origin

        app.UseCors(policy => policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());

        app.MapControllers();
        EasyLearnDbInitializer.Seed(app);

        app.Run();
    }
}
