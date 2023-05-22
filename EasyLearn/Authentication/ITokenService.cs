using EasyLearn.Models.DTOs.UserDTOs;

namespace EasyLearn.Authentication;

public interface ITokenService
{
    string CreateToken(JWTokenRequestModel model);
}
