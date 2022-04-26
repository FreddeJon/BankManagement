namespace Application.Features.Statistics.Query;
public class GetStatisticsQueryHandler : IRequestHandler<GetStatisticsQuery, StatisticsBaseResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetStatisticsQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<StatisticsBaseResponse> Handle(GetStatisticsQuery request, CancellationToken cancellationToken)
    {
        var response = new StatisticsBaseResponse();

        var query = _context.Customers.Include(x => x.Accounts).AsQueryable();
        var totalCount = await query.CountAsync(cancellationToken: cancellationToken);

        response.Overview = await CreateStatistic(query, totalCount, "");
        response.Sweden = await CreateStatistic(query, totalCount, "SE");
        response.Finland = await CreateStatistic(query, totalCount, "FI");
        response.Norway = await CreateStatistic(query, totalCount, "NO");





        var validCountryCodes = new[] { "SE", "FI", "NO" };


        if (request.CountryCode != null && validCountryCodes.Any(x => x.Contains(request.CountryCode.ToUpper())))
        {
            query = query.Where(x => x.CountryCode == request.CountryCode);
        }


        response.Customers = _mapper.Map<IReadOnlyList<CustomerDto>>(await query
            .OrderByDescending(x => x.Accounts.Sum(b => b.Balance)).Take(10).ToListAsync(cancellationToken: cancellationToken));

        return response;

    }




    private static async Task<Statistic> CreateStatistic(IQueryable<Domain.Entities.Customer> query, int totalAmountOfCustomers, string countryCode)
    {
        var customers = string.IsNullOrWhiteSpace(countryCode) ? await query.ToListAsync() : await query.Where(x => x.CountryCode == countryCode).ToListAsync();




        var accounts = customers.SelectMany(x => x.Accounts).ToList();
        var totalAccounts = accounts.Count;
        var totalBalance = accounts.Sum(x => x.Balance);
        var totalCustomers = customers.Count;
        var percentage = (double)totalCustomers / totalAmountOfCustomers;

        var country = countryCode switch
        {
            "SE" => "Sweden",
            "NO" => "Norway",
            "FI" => "Finland",
            _ => "Overview"
        };

        var response = new Statistic()
        {
            Country = country,
            CountryCode = countryCode,
            TotalAccounts = totalAccounts,
            TotalBalance = totalBalance,
            TotalCustomers = totalCustomers,
            Percentage = percentage
        };


        return response;
    }
}