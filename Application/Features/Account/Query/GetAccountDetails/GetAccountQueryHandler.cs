namespace Application.Features.Account.Query.GetAccountDetails;
// ReSharper disable once UnusedType.Global
public class GetAccountQueryHandler : IRequestHandler<GetAccountAndCustomerQuery, GetAccountResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAccountQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<GetAccountResponse> Handle(GetAccountAndCustomerQuery request, CancellationToken cancellationToken)
    {
        var response = new GetAccountResponse();
        var customer = await _context.Customers.Include(x => x.Accounts).ThenInclude(x => x.Transactions.OrderByDescending(t => t.Date)).FirstOrDefaultAsync(x => x.Id == request.CustomerId, cancellationToken: cancellationToken);

        if (customer is null)
        {
            response.Status = StatusCode.Error;
            response.StatusText = $"Customer with id: {request.CustomerId} not found";
            return response;
        }

        var account = customer.Accounts.FirstOrDefault(x => x.Id == request.AccountId);

        if (account is null)
        {
            response.Status = StatusCode.Error;
            response.StatusText = $"Account with id: {request.CustomerId} not found";
            return response;
        }

        response.TotalTransactions = account.Transactions.Count;
        response.TotalPages = (int)Math.Ceiling((double)account.Transactions.Count / 20);
        account.Transactions = account.Transactions.Take(20).ToList();
        response.Customer = _mapper.Map<CustomerDto>(customer);
        response.Account = _mapper.Map<AccountDto>(account);
        return response;
    }
}