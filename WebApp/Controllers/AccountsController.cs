namespace WebApp.Controllers;

[Authorize(Roles = nameof(ApplicationRoles.Customer), AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public AccountsController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var customerId = HttpContext.User.SubjectId();

        return Ok();
    }
}