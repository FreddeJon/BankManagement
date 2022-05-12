using Microsoft.VisualBasic;

namespace Application.Features.Api.Query.GetMe;
public class GetMeQueryHandler : IRequestHandler<GetMeQuery, GetMeResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetMeQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<GetMeResponse> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        var response = new GetMeResponse();

        var user = await _context.CustomerUser.FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken: cancellationToken);
        var customer = _mapper.Map<CustomerDto>(await _context.Customers
            .Include(x => x.Accounts)
            .ThenInclude(x => x.Transactions)
            .FirstOrDefaultAsync(x => x.Id == user.CustomerId, cancellationToken: cancellationToken));




        if (customer is null)
        {
            return response;
        }


        response.Customer = customer;

        return response;
    }


}
