

namespace Application.Features.Customer.Query.GetCustomerByIdIncludeAccounts;
public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, GetCustomerBaseResponse>
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public GetCustomerQueryHandler(IMapper mapper, ApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    public async Task<GetCustomerBaseResponse> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var response = new GetCustomerBaseResponse
        {
            Customer = _mapper.Map<CustomerDto>(await _context.Customers.Include(a => a.Accounts)
            .ThenInclude(x => x.Transactions.OrderBy(t => t.Date)).FirstOrDefaultAsync(x => x.Id == request.CustomerId,
                cancellationToken: cancellationToken))
        };


        if (response.Customer is not null) return response;

        response.Status = StatusCode.Error;
        response.StatusText = $"Customer with id: {request.CustomerId} not found";
        return response;
    }
}