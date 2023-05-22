using BCrypt.Net;
using EasyLearn.Authentication;
using EasyLearn.GateWays.Email;
using EasyLearn.GateWays.FileManager;
using EasyLearn.Models.DTOs;
using EasyLearn.Models.DTOs.EmailSenderDTOs;
using EasyLearn.Models.DTOs.UserDTOs;
using EasyLearn.Models.Entities;
using EasyLearn.Models.Enums;
using EasyLearn.Repositories.Interfaces;
using EasyLearn.Services.Interfaces;
using Mapster;

namespace EasyLearn.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly ISendInBlueEmailService _emailService;
    private readonly IFileManagerService _fileManagerService;
    public UserService(IUserRepository userRepository, ISendInBlueEmailService emailService, IFileManagerService fileManagerService, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _fileManagerService = fileManagerService;
        _tokenService = tokenService;
    }
    public async Task<BaseResponse<UserDTO>> DeleteUser(string id, string userId)
    {

        var user = await _userRepository.GetAsync(x => x.Id == id && !x.IsDeleted && x.IsActive);
        if (user == null)
        {
            return new BaseResponse<UserDTO>
            {
                Message = "User not Found...",
                Success = false,
            };
        }
        user.IsDeleted = true;
        user.DeletedOn = DateTime.Now;
        user.DeletedBy = userId;

        await _userRepository.SaveChangesAsync();
        return new BaseResponse<UserDTO>
        {
            Message = "User successfully deleted...",
            Success = true,
        };

    }

    public async Task<BaseResponse<ICollection<UserDTO>>> GetAllUser()
    {
        var users = await _userRepository.GetListAsync(x => x.RoleId != "Admin" && !x.IsDeleted);

        if (users.Count() == 0)
        {
            return new BaseResponse<ICollection<UserDTO>>
            {
                Message = "User not found..",
                Success = false,
            };
        }
        var userModel = new BaseResponse<ICollection<UserDTO>>
        {
            Success = true,
            Message = "Details successfully retrieved...",
            Data = users.Adapt<IList<UserDTO>>(),
        };
        return userModel;
    }

    public async Task<BaseResponse<UserDTO>> GetUserById(string id)
    {
        var user = await _userRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (user == null)
        {
            return new BaseResponse<UserDTO>
            {
                Message = "User not found...",
                Success = false,
            };
        }
        var userDto = new BaseResponse<UserDTO>
        {
            Message = "Details successfully retrieved...",
            Success = true,
            Data = user.Adapt<UserDTO>(),
        };
        return userDto;
    }

    public async Task<BaseResponse<UserDTO>> UpdateUserProfile(UpdateUserProfileRequestModel model, string userId)
    {
        var user = await _userRepository.GetAsync(x => x.Id == userId && x.IsActive && !x.IsDeleted);
        if (user == null)
        {
            return new BaseResponse<UserDTO>
            {
                Message = "User does not exist",
                Success = false,

            };
        }


        user.FirstName = model.FirstName ?? user.FirstName;
        user.LastName = model.LastName ?? user.LastName;
        user.Biography = model.Biography ?? user.Biography;
        user.Skill = model.Skill ?? user.Skill;
        user.Interest = model.Interest ?? user.Interest;
        user.PhoneNumber = model.PhoneNumber ?? user.PhoneNumber;
        user.StudentshipStatus = model.StudentshipStatus != 0 ? StudentshipStatus.Student : StudentshipStatus.Graduate;
        user.ModifiedOn = DateTime.Now;
        user.ModifiedBy = userId;
        await _userRepository.SaveChangesAsync();
        return new BaseResponse<UserDTO>
        {
            Message = "Account Updated successfully",
            Success = true,
            Data = user.Adapt<UserDTO>(),
        };
    }


    public async Task<BaseResponse<UserDTO>> UpgradeUserRole(UserUpgradeRequestModel model, string userId)
    {
        var user = await _userRepository.GetFullDetails(x => x.Id == model.UserId);
        if (user == null)
        {
            return new BaseResponse<UserDTO>
            {
                Message = "User not found...",
                Success = false,
            };
        }
        if (user.RoleId.ToLower() == "student")
        {
            if (model.RoleId.ToLower() == "moderator")
            {
                var moderatorUser = new Moderator
                {
                    Id = user.Student.Id,
                    UserId = user.Id,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedBy = userId,
                };
                user.RoleId = "Moderator";
                user.Moderator = moderatorUser;
                //await _userRepository.AddAsync(moderatorUser);
                await _userRepository.SaveChangesAsync();
                return new BaseResponse<UserDTO>
                {
                    Message = "User successfully upgraded to a moderator...",
                    Success = true,
                };
            }
            if (model.RoleId.ToLower() == "instructor")
            {
                var instructorUser = new Instructor
                {
                    Id = user.Student.Id,
                    UserId = user.Id,
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedBy = userId,
                    VerifyBy = userId,
                    VerifyOn = DateTime.Now,

                };
                user.RoleId = "Instructor";
                user.Instructor = instructorUser;
                await _userRepository.SaveChangesAsync();
                return new BaseResponse<UserDTO>
                {
                    Message = "User successfully upgraded to an instructor...",
                    Success = true,
                };
            }
            return new BaseResponse<UserDTO>
            {
                Message = "User can not be downgraded...",
                Success = false,
            };
        }
        if (user.RoleId.ToLower() == "instructor" && model.RoleId.ToLower() == "moderator")
        {
            var moderatorUser = new Moderator
            {
                Id = user.Instructor.Id,
                UserId = user.Id,
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedBy = userId,
            };
            user.RoleId = "Moderator";
            user.Moderator = moderatorUser;
            await _userRepository.SaveChangesAsync();
            return new BaseResponse<UserDTO>
            {
                Message = "User successfully upgraded to a moderator...",
                Success = true,
            };
        }
        return new BaseResponse<UserDTO>
        {
            Message = "User can not be downgraded...",
            Success = false,
        };
    }

    public async Task<BaseResponse<UserDTO>> UserRegistration(CreateUserRequestModel model, string baseUrl, string userId)
    {
        if (model.Password == null)
            return new BaseResponse<UserDTO>
            {
                Success = false,
                Message = "Password can not be empty..",
            };

        var emailExist = await _userRepository.ExistByEmailAsync(model.Email);
        if (emailExist)
        {
            return new BaseResponse<UserDTO>
            {
                Success = false,
                Message = "Email already exit..",
            };
        }
        var user = model.Adapt<User>();
        //if (model.FormFile != null)
        //{
        //    user.ProfilePicture = await _fileManagerService.GetFileName(model.FormFile, "uploads", "images", "profilePictures");
        //}
        //else
        //{
        //    user.ProfilePicture = "default.jpg";
        //}
        user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password, SaltRevision.Revision2Y);
        user.UserName = $"{model.Email.Remove(model.Email.IndexOf('@'))}{new Random().Next(100, 999)}";
        user.RoleId = "Student";



        var userStudent = new Student
        {
            UserId = user.Id,
            CreatedBy = user.CreatedBy,
            CreatedOn = user.CreatedOn,
        };
        var senderDetail = new EmailSenderDetails
        {
            EmailToken = $"{baseUrl}/User/ConfirmEmail?emailToken={user.EmailToken}",
            ReceiverEmail = user.Email,
            ReceiverName = model.FirstName,
        };
        await _emailService.EmailVerificationTemplate(senderDetail, baseUrl);
        user.Student = userStudent;
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
        return new BaseResponse<UserDTO>
        {
            Success = true,
            Message = "Account successfuly created..",
        };
    }


    public async Task<BaseResponse<LoginModel>> Login(LoginRequestModel model)
    {
        var user = await _userRepository.GetFullDetails(x => x.UserName.ToLower() == model.Email.ToLower() || x.Email.ToLower() == model.Email.ToLower());
        if (user == null)
        {
            return new BaseResponse<LoginModel>
            {
                Message = "invalid login details",
                Success = false,
            };
        }
        if (!user.EmailConfirmed || !user.IsActive)
        {
            return new BaseResponse<LoginModel>
            {
                Message = "kindly Confirm your email...",
                Success = false,
            };
        }

        try
        {
            var verifyPassword = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);
            if (!verifyPassword)
            {
                return new BaseResponse<LoginModel>
                {
                    Message = "incorrect login detail...",
                    Success = false,
                };
            }
        }
        catch (Exception)
        {
            return new BaseResponse<LoginModel>
            {
                Message = "Advice to reset your password...",
                Success = false,
            };
        }
        var jwtModel = user.Adapt<JWTokenRequestModel>();
        jwtModel.UserId = user.Admin?.Id ?? user.Moderator?.Id ?? user.Instructor?.Id ?? user.Student?.Id;
        var accessToken = _tokenService.CreateToken(jwtModel);
        var loginModel = new BaseResponse<LoginModel>
        {
            Message = $"logged successfully. Wellcome back! Dear{user.FirstName}..",
            Success = true,
            Data = user.Adapt<LoginModel>(),
        };
        loginModel.Data.JWToken = accessToken;
        return loginModel;
    }

    public async Task<BaseResponse<UserDTO>> EmailVerification(string emailToken, string userId)
    {
        var user = await _userRepository.GetAsync(x => x.EmailToken == emailToken);
        if (user == null)
        {
            return new BaseResponse<UserDTO>
            {
                Message = "Wrong verification code...",
                Success = false,
            };
        }
        if (user.EmailConfirmed)
        {
            return new BaseResponse<UserDTO>
            {
                Message = "Account already verified, proceed to login...",
                Success = false,
            };
        }
        user.EmailConfirmed = true;
        user.ModifiedOn = DateTime.Now;
        user.ModifiedBy = userId;
        await _userRepository.SaveChangesAsync();
        return new BaseResponse<UserDTO>
        {
            Message = "Account activated...",
            Success = true,
        };
    }

    public async Task<BaseResponse<UserDTO>> PasswordReset(string email, string baseUrl)
    {
        var user = await _userRepository.GetAsync(x => x.Email.ToLower() == email.ToLower() || email.ToLower() == x.UserName.ToLower());
        if (user == null)
        {
            return new BaseResponse<UserDTO>
            {
                Success = false,
                Message = "Link has been sent to you if your email registerd with us..",
            };
        }
        user.EmailToken = Guid.NewGuid().ToString().Replace('-', 'P');
        user.EmailConfirmed = false;
        user.ModifiedOn = DateTime.Now;

        var senderDetail = new EmailSenderDetails
        {
            EmailToken = $"{baseUrl}/User/ConfirmPasswordReset?emailToken={user.EmailToken}",
            ReceiverEmail = user.Email,
            ReceiverName = user.FirstName,
        };
        var emailSender = _emailService.EmailVerificationTemplate(senderDetail, baseUrl);
        await _userRepository.SaveChangesAsync();
        return new BaseResponse<UserDTO>
        {
            Success = true,
            Message = "Link has been sent to you if your email registerd with us..",
        };

    }
    public async Task<BaseResponse<UserDTO>> ConfirmPasswordReset(string emailToken, string userId)
    {
        var user = await _userRepository.GetAsync(x => x.EmailToken == emailToken);
        if (user == null)
        {
            return new BaseResponse<UserDTO>
            {
                Message = "Wrong verification code...",
                Success = false,
            };
        }
        var resetDate = Convert.ToDateTime(user.ModifiedOn);
        var expiryDate = resetDate.AddDays(1);

        if (expiryDate < DateTime.Now)
        {
            return new BaseResponse<UserDTO>
            {
                Message = "Link has expired kindly request for new reset link...",
                Success = false,
            };
        }
        return new BaseResponse<UserDTO>
        {
            Message = "Token confirmed activated...",
            Success = true,
            Data = user.Adapt<UserDTO>(),
        };
    }
}
