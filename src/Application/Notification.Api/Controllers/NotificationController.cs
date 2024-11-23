using Common.DTOs.Notification;
using Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Notification.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationController : ControllerBase
{
    private readonly ISmsRule _smsRule;

    public NotificationController(ISmsRule smsRule)
    {
        _smsRule = smsRule;
    }
    
    [HttpPost]
    public IActionResult Post(SmsDto dto)
    {
        try
        {
            _smsRule.Send(dto);
            
            return Accepted();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
    
    [HttpGet]
    public IActionResult Get([FromQuery] string identifier)
    {
        try
        {
            var result = _smsRule.GetByIdentifier(identifier);
            
            return Ok(result);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
}