using EasyLearn.Models.DTOs.UserDTOs;
using EasyLearn.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EasyLearn.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }


    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> UserResgistration([FromForm] CreateUserRequestModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (model == null) return BadRequest(ModelState);
        var loggedInUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var baseUrl = $"https://{Request.Host}";
        var user = await _userService.UserRegistration(model, baseUrl, loggedInUser);
        if (!user.Success) return BadRequest(user.Message);
        return Ok(user);

    }

    [HttpGet]
    [Route("{id}")]
    [ActionName("GetUserById")]
    public async Task<IActionResult> GetUserById([FromRoute] string id)
    {
        var user = await _userService.GetUserById(id);
        if (!user.Success)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpGet]
    [Route("ListAllUser")]
    public async Task<IActionResult> GetAllUser()
    {
        var users = await _userService.GetAllUser();
        if (!users.Success)
        {
            return Ok(users);
        }
        return Ok(users);
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateUserProfile(UpdateUserProfileRequestModel model)
    {
        var loggedInUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userService.UpdateUserProfile(model, loggedInUser);
        if (!user.Success)
        {
            return Ok(user);
        }
        return CreatedAtAction("GetUserById", new { id = user.Data.Id }, user);
    }

    [HttpPut]
    [Route("Delete/{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var loggedInUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userService.GetUserById(id);
        if (!user.Success)
        {
            return NotFound();
        }
        await _userService.DeleteUser(id, loggedInUser);
        return Ok();
    }
}
