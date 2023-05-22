using EasyLearn.Models.DTOs.UserDTOs;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EasyLearn.Authentication;

public class TokenService : ITokenService
{
    private const int ExpirationMinutes = 30;
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
                    new Claim(JwtRegisteredClaimNames.Sub, "TokenForTheApiWithAuth"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                    new Claim(ClaimTypes.NameIdentifier, model.Id),
                    new Claim(ClaimTypes.Name, model.FirstName),
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