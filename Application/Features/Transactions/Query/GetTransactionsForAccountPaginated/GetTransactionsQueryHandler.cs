namespace Application.Features.Transactions.Query.GetTransactionsForAccountPaginated;

public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, GetTransactionsResponse>
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public GetTransactionsQueryHandler(IMapper mapper, ApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<GetTransactionsResponse> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        var response = new GetTransactionsResponse();
        var account = await _context.Accounts.Include(x => x.Transactions.OrderByDescending(t => t.Date))
            .FirstOrDefaultAsync(x => x.Id == request.AccountId, cancellationToken: cancellationToken);
        if (account is null)
        {
            response.Status = StatusCode.Error;
            response.StatusText = "Account not found";
            return response;
        }


        response.Page = request.Page;
        response.TotalPages = (int)Math.Ceiling((double)account.Transactions.Count / 20);
        response.Transactions =
            _mapper.Map<IReadOnlyList<TransactionDto>>(account.Transactions.Skip((request.Page - 1) * 20).Take(20));
        return response;
    }
}