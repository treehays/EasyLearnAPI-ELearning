using EasyLearn.Authentication;
using EasyLearn.Models.DTOs.UserDTOs;
using EasyLearn.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EasyLearn.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public UserController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    //tested
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("UserRegistration")]
    public async Task<IActionResult> Register([FromBody] CreateUserRequestModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (model == null) return BadRequest(ModelState);
        var loggedInUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var baseUrl = $"https://{Request.Host}";
        var user = await _userService.UserRegistration(model, baseUrl, loggedInUser);
        if (!user.Success) return BadRequest(user);
        return Ok(user);
    }

    [HttpPatch("EmailVerification/{emailToken}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EmailVerification([FromRoute] string emailToken)
    {
        if (emailToken == null) return BadRequest();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var response = await _userService.EmailVerification(emailToken, userId);
        if (!response.Success) return BadRequest(response);
        return Ok(response);
    }

    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (model == null) return BadRequest("Bad credentials");
        var response = await _userService.Login(model);
        if (!response.Success) return Unauthorized(response);
        //this will be refactor later
        return Ok(response);
    }

    [HttpPatch("RequestPasswordReset")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RequestPasswordReset([FromQuery] string email)
    {
        if (email == null) return BadRequest();
        var baseUrl = $"https://{Request.Host}";
        var response = await _userService.PasswordReset(email, baseUrl);
        if (!response.Success) return BadRequest(response);
        return Ok(response);

    }

    [HttpPatch("ConfirmPasswordReset/{emailToken}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfirmPasswordReset([FromRoute] string emailToken)
    {
        if (emailToken == null) return BadRequest();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userService.ConfirmPasswordReset(emailToken, userId);
        if (!user.Success) return BadRequest(user);
        return Ok(user);
    }

    //[HttpPatch("ResetPassword")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> ResetPassword([FromQuery] string emailToken, [FromQuery] string email)
    //{
    //    if (emailToken == null) return BadRequest();
    //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    //    var user = await _userService.ConfirmPasswordReset(emailToken, userId);
    //    if (!user.Success) return BadRequest(user);
    //    return Ok(user);
    //}
    [HttpPatch("UpgradeUserRole")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpgradeUserRole(UserUpgradeRequestModel model, string roleId)
    {
        if (model == null || roleId == null) return BadRequest();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var response = await _userService.UpgradeUserRole(model, userId);
        if (!response.Success) return BadRequest(response);
        return Ok(response);
    }


    //tested
    [HttpGet(Name = "GetAll"), Authorize]
    //[HttpGet(Name = "GetAll"), Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll()
    {
        // Retrieve the authorization token from the request headers
        string authorizationHeader = Request.Headers["Authorization"];

        // Extract the token value from the authorization header (e.g., removing "Bearer ")
        string token = authorizationHeader.Replace("Bearer ", "");

        var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var response = await _userService.GetAllUser();
        if (!response.Success) return NotFound(response);
        return Ok(response);
    }

    //tested
    [HttpGet("GetById/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById([FromRoute] string userId)
    {
        if (userId == null) return BadRequest();
        var user = await _userService.GetUserById(userId);
        if (!user.Success) return NotFound(user);
        return Ok(user);
    }

    [HttpPut("UpdateProfile/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserProfile([FromRoute] string userId, [FromBody] UpdateUserProfileRequestModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (userId == null || model == null) return BadRequest();
        var loggedInUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userService.UpdateUserProfile(model, userId);
        if (!user.Success) return Ok(user.Message);
        return CreatedAtAction(nameof(GetById), new { userId = user.Data.Id }, user);
    }



    //[Route("Delete/{userId}")]
    [HttpPatch("{userId}/Delete")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var loggedInUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userService.DeleteUser(userId, loggedInUser);
        if (!user.Success) return NotFound(user);
        return Ok(user);
    }

}
