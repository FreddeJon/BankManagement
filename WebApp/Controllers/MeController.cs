using System.Security.Claims;
using Application.Features.Api.Query.GetMe;
using Application.Options;
using Microsoft.Extensions.Options;

namespace WebApp.Controllers;


[Authorize(Roles = nameof(ApplicationRoles.Customer), AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
public class MeController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public MeController(IOptions<JwtOptions> options, IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Me()
    {
        var id = HttpContext.User.SubjectId();
        var response = await _mediator.Send(new GetMeQuery() { UserId = id });
        if (response.Status == Application.Responses.StatusCode.Error)
        {
            return BadRequest(response.StatusText);
        }

        return Ok(_mapper.Map<MeCustomerViewModel>(response.Customer));
    }




    public class MeCustomerViewModel
    {
        public int Id { get; set; }
        public string NationalId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Streetaddress { get; set; } = null!;
        public string Telephone { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string Zipcode { get; set; } = null!;
        public decimal Balance { get; set; }
        public List<MeAccountViewModel> Accounts { get; set; } = new();
    }

    public class MeAccountViewModel
    {
        public int Id { get; set; }
        public string AccountType { get; set; } = null!;
        public decimal Balance { get; set; }
        public DateTime? LatestTransaction { get; set; }
    }
}

public static class ContextHelper
{
    public static string SubjectEmail(this ClaimsPrincipal user)
    {
        return user?.Claims?.FirstOrDefault(c => c.Type.Contains("emailaddress"))?.Value!;
    }

    public static string SubjectId(this ClaimsPrincipal user)
    {
        return user?.Claims?.FirstOrDefault(c => c.Type.Contains("nameidentifier"))?.Value!;
    }
}
