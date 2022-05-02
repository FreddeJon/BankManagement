using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace WebApp.Controllers;


[Authorize(Roles = nameof(ApplicationRoles.Customer), AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
public class MeController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _context;
    private readonly JwtOptions _options;

    public MeController(IOptions<JwtOptions> options, IMediator mediator, ApplicationDbContext context)
    {
        _mediator = mediator;
        _context = context;
        _options = options.Value;
    }

    [HttpGet]
    public async Task<IActionResult> Me()
    {
        var id = HttpContext.User.SubjectId();
        
        var customer = _context.Customers.Include(x => x.Accounts).FirstOrDefault(x => x.Id == _context.CustomerUser.FirstOrDefault(x => x.UserId == id)!.CustomerId);

        
        return Ok(customer);
    }


    
}

public static class ContextHelper
{
    public static string SubjectEmail(this ClaimsPrincipal user) => user?.Claims?.FirstOrDefault(c => c.Type.Contains("emailaddress"))?.Value;
    public static string SubjectId(this ClaimsPrincipal user) => user?.Claims?.FirstOrDefault(c => c.Type.Contains("nameidentifier"))?.Value;
}
