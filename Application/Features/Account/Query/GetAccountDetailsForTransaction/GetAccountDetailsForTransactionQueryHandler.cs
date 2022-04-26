namespace Application.Features.Account.Query.GetAccountDetailsForTransaction;

// ReSharper disable once UnusedType.Global
public class GetAccountDetailsForTransactionQueryHandler : IRequestHandler<GetAccountDetailsForTransactionQuery,
    GetAccountDetailsForTransactionResponse>
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public GetAccountDetailsForTransactionQueryHandler(IMapper mapper, ApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    public async Task<GetAccountDetailsForTransactionResponse> Handle(GetAccountDetailsForTransactionQuery request, CancellationToken cancellationToken)
    {
        var response = new GetAccountDetailsForTransactionResponse();
        var customer = await _context.Customers.Include(x => x.Accounts).FirstOrDefaultAsync(x => x.Id == request.CustomerId, cancellationToken: cancellationToken);
        if (customer is null)
        {
            response.Status = StatusCode.Error;
            response.StatusText = "Customer not found";
            return response;
        }
        var account = customer.Accounts.FirstOrDefault(x => x.Id == request.AccountId);
        if (account is null)
        {
            response.Status = StatusCode.Error;
            response.StatusText = "Customer not found";
            return response;
        }


        response.CustomerName = $"{customer.Givenname} {customer.Surname}";
        response.CustomerId = customer.Id;
        response.Account = _mapper.Map<AccountDto>(account);
        return response;
    }
}