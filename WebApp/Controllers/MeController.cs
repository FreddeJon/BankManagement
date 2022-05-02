using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace WebApp.Controllers;


[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
public class MeController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly JwtOptions _options;

    public MeController(IOptions<JwtOptions> options, IMediator mediator)
    {
        _mediator = mediator;
        _options = options.Value;
    }


    [HttpGet]
    public async Task<IActionResult> Me()
    {
        var email = HttpContext.User.SubjectId();

        return Ok(email);
    }


    
}

public static class ContextHelper
{
    public static string SubjectId(this ClaimsPrincipal user) => user?.Claims?.FirstOrDefault(c => c.Type.Contains("emailaddress"))?.Value;
}
