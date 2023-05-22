using EasyLearn.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EasyLearn.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EnumController : ControllerBase
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("GetGender")]
    public IActionResult GetGender()
    {
        //var gender = Enum.GetValues(typeof(Gender)).Cast<int>().ToList();
        //var statuss = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToDictionary(e => e.ToString(), e => (int)e);
        var gender = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToDictionary(x => x.ToString(), x => (int)x);
        return Ok(gender);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("BoolOption")]
    public IActionResult BoolOption()
    {
        var boolOption = Enum.GetValues(typeof(BoolOption)).Cast<BoolOption>().ToDictionary(x => x.ToString(), x => (int)x);
        return Ok(boolOption);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("CompletionStatus")]
    public IActionResult CompletionStatus()
    {
        var completionStatus = Enum.GetValues(typeof(CompletionStatus)).Cast<CompletionStatus>().ToDictionary(x => x.ToString(), x => (int)x);
        return Ok(completionStatus);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("CourseLanguage")]
    public IActionResult CourseLanguage()
    {
        var courseLanguage = Enum.GetValues(typeof(CourseLanguage)).Cast<CourseLanguage>().ToDictionary(x => x.ToString(), x => (int)x);
        return Ok(courseLanguage);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("DifficultyLevel")]
    public IActionResult DifficultyLevel()
    {
        var difficultyLevel = Enum.GetValues(typeof(DifficultyLevel)).Cast<DifficultyLevel>().ToDictionary(x => x.ToString(), x => (int)x);
        return Ok(difficultyLevel);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("PaymentMethods")]
    public IActionResult PaymentMethods()
    {
        var paymentMethods = Enum.GetValues(typeof(PaymentMethods)).Cast<PaymentMethods>().ToDictionary(x => x.ToString(), x => (int)x);
        return Ok(paymentMethods);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("PaymentStatus")]
    public IActionResult PaymentStatus()
    {
        var paymentStatus = Enum.GetValues(typeof(PaymentStatus)).Cast<PaymentStatus>().ToDictionary(x => x.ToString(), x => (int)x);
        return Ok(paymentStatus);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("StudentshipStatus")]
    public IActionResult StudentshipStatus()
    {
        var studentshipStatus = Enum.GetValues(typeof(StudentshipStatus)).Cast<StudentshipStatus>().ToDictionary(x => x.ToString(), x => (int)x);
        return Ok(studentshipStatus);
    }

}
