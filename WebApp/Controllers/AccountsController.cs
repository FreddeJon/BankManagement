using Application.Features.Api.Query.GetAccountForCustomer;
// ReSharper disable ClassNeverInstantiated.Local

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
    public async Task<IActionResult> GetById(int id, int limit = 5, int offset = 0)
    {
        var userId = HttpContext.User.SubjectId();


        var take = limit is < 1 or > 20 ? 5 : limit;
        var skip = offset < 0 ? 0 : offset;


        var response = await _mediator.Send(new GetAccountForCustomerQuery() { UserId = userId, AccountId = id, Limit = take, Offset = skip });
        if (response.Status == Application.Responses.StatusCode.Error)
        {
            return BadRequest(response.StatusText);
        }
        return Ok(_mapper.Map<AccountViewModel>(response.Account));
    }

    public class AccountViewModel
    {
        public int Id { get; init; }
        public string AccountType { get; init; } = null!;
        public DateTime Created { get; init; }
        public decimal Balance { get; init; }
        // ReSharper disable once CollectionNeverUpdated.Global
        public List<TransactionViewModel> Transactions { get; init; } = new();
    }
    public class TransactionViewModel
    {
        public int Id { get; init; }
        public string Type { get; init; } = null!;
        public string Operation { get; init; } = null!;
        public DateTime Date { get; init; }
        public string Amount { get; init; } = null!;
        public string NewBalance { get; init; } = null!;
    }
}