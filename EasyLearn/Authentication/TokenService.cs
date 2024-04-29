using EasyLearn.Models.DTOs.UserDTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EasyLearn.Authentication;

public class TokenService : ITokenService
{
	//private readonly SecuritySettings _securitySettings;
	//private readonly JwtSettings _jwtSettings;

    //public TokenService(IOptions<JwtSettings> jwtSettings, IOptions<SecuritySettings> securitySettings)
    //{
    //    _jwtSettings = jwtSettings.Value;
    //    _securitySettings = securitySettings.Value;
    //}

    private const int ExpirationMinutes = 300;
    public string CreateToken(JWTokenRequestModel model)
    {
        var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
        var token = CreateJwtToken(CreateClaims(model), CreateSigningCredentials(), expiration);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials, DateTime expiration) => new JwtSecurityToken("EasyLearnAuthIssuer", "EasyLearnAuthAudience", claims, expires: expiration, signingCredentials: credentials);

    private List<Claim> CreateClaims(JWTokenRequestModel model)
    {
        try
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.FirstName),
                    new Claim("testingkey", "TokenForTheApiWithAuth"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                    new Claim(ClaimTypes.GivenName, model.Id),
                    new Claim(ClaimTypes.Role, model.RoleId.ToLower()),
                    new Claim(ClaimTypes.Actor, model.UserId),
                    new Claim(ClaimTypes.UserData, model.ProfilePicture)
                };
            return claims;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    private SigningCredentials CreateSigningCredentials()
    {
        var signingCredentials = new SigningCredentials(
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("!SomethingSecret!")), SecurityAlgorithms.HmacSha256);
        return signingCredentials;
    }
}

/*

internal static class Startup
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<SecuritySettings>(config.GetSection(nameof(SecuritySettings)));
        services.AddJwtAuth(config);
        return services;
    }
    internal static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<JwtSettings>(config.GetSection($"SecuritySettings:{nameof(JwtSettings)}"));
        var jwtSettings = config.GetSection($"SecuritySettings:{nameof(JwtSettings)}").Get<JwtSettings>();
        if (string.IsNullOrEmpty(jwtSettings.SecurityKey))
            throw new InvalidOperationException("No SecurityKey defined in JwtSettings config.");
       // byte[] key = Encoding.ASCII.GetBytes(jwtSettings.SecurityKey);

        services
            .AddAuthentication(authentication =>
            {
                authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.ValidIssuer,
                    ValidAudience = jwtSettings.ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.SecurityKey)
             ),
                 };
             });
        return services;
    }
}


public class SecuritySettings
{
    public JwtSettings JwtSettings { get; set; }
    public string RootUserEmail { get; set; }
    public string DefaultPassword { get; set; }
    public bool RequireConfirmedAccount { get; set; }
}
public class JwtSettings
{
    public string SecurityKey { get; set; }
    public string ValidAudience { get; set; }
    public string ValidIssuer { get; set; }
    public int ExpirationInDays { get; set; }
}
*/