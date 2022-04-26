using Application.Infrastructure.Paging;

namespace Application.Features.Customer.Query.GetCustomersPaginated;
public class GetCustomersPaginatedQueryHandler : IRequestHandler<GetCustomerPaginatedQuery, GetCustomersPaginatedBaseResponse>
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public GetCustomersPaginatedQueryHandler(IMapper mapper, ApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    public async Task<GetCustomersPaginatedBaseResponse> Handle(GetCustomerPaginatedQuery request, CancellationToken cancellationToken)
    {
        var response = new GetCustomersPaginatedBaseResponse();
        var query = _context.Customers.Include(x => x.Accounts).AsQueryable();

        if (!string.IsNullOrEmpty(request.Search))
        {
            request.Search = request.Search.ToLower();
            query = query.Where(x => x.Country.ToLower().Contains(request.Search) ||
                                     x.City.ToLower().Contains(request.Search) ||
                                     x.Streetaddress.ToLower().Contains(request.Search) ||
                                     (x.Givenname + " " + x.Surname).ToLower().Contains(request.Search));
        }


        if (request.SortColumn != nameof(CustomerDto.Balance))
        {
            query = query.HandleSorting(request.SortingOrder, request.SortColumn);
        }
        else
        {
            query = request.SortingOrder == SortingOrder.Asc ? query.OrderBy(x => x.Accounts.Sum(a => a.Balance)) : query.OrderByDescending(x => x.Accounts.Sum(a => a.Balance));
        }

        var pageResponse = await query.HandlePaging(request.Page, request.Limit);

        response.Limit = pageResponse.Limit;
        response.Page = pageResponse.Page;
        response.TotalPage = pageResponse.TotalPage;


        response.Status = StatusCode.Success;
        response.Customers = _mapper.Map<IReadOnlyList<CustomerDto>>(pageResponse.Results);

        return response;
    }
}