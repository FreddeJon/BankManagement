namespace Application.Features.Api.Query.GetAccountForCustomer;
public class GetAccountForCustomerQueryHandler : IRequestHandler<GetAccountForCustomerQuery, GetAccountForCustomerResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAccountForCustomerQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<GetAccountForCustomerResponse> Handle(GetAccountForCustomerQuery request, CancellationToken cancellationToken)
    {
        var response = new GetAccountForCustomerResponse();
        var user = await _context.CustomerUser.FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken: cancellationToken);
        var customer = _mapper.Map<CustomerDto>(await _context.Customers
            .Include(x => x.Accounts)
            .ThenInclude(x => x.Transactions)
            .FirstOrDefaultAsync(x => x.Id == user!.CustomerId, cancellationToken: cancellationToken));

        if (customer is null)
        {
            response.Status = StatusCode.Error;
            response.StatusText = $"Customer with id: {request.UserId} not found";
            return response;
        }

        var account = customer.Accounts.FirstOrDefault(x => x.Id == request.AccountId);

        if (account is null)
        {
            response.Status = StatusCode.Error;
            response.StatusText = $"Account with id: {request.AccountId} not found in your accounts";
            return response;
        }

        response.TotalTransactions = account.Transactions.Count;
        response.TotalPages = (int)Math.Ceiling((double)account.Transactions.Count / 20);
        account.Transactions = account.Transactions.OrderByDescending(x => x.Date).Skip(request.Offset).Take(request.Limit).ToList();
        response.Account = _mapper.Map<AccountDto>(account);
        return response;
    }
}
