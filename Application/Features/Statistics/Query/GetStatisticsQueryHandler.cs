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

        var customers = await _context.Customers.Include(x => x.Accounts).ToListAsync();
        var totalCount = customers.Count;

        response.Overview = await CreateStatistic(customers, totalCount, "");
        response.Sweden = await CreateStatistic(customers, totalCount, "SE");
        response.Finland = await CreateStatistic(customers, totalCount, "FI");
        response.Norway = await CreateStatistic(customers, totalCount, "NO");





        var validCountryCodes = new[] { "SE", "FI", "NO" };


        if (request.CountryCode != null && validCountryCodes.Any(x => x.Contains(request.CountryCode.ToUpper())))
        {
            customers = customers.Where(x => x.CountryCode == request.CountryCode).ToList();
        }


        response.Customers = _mapper.Map<IReadOnlyList<CustomerDto>>(customers
            .OrderByDescending(x => x.Accounts.Sum(b => b.Balance)).Take(10).ToList());

        return response;
         
    }




    private static Task<Statistic> CreateStatistic(List<Domain.Entities.Customer> customers, int totalAmountOfCustomers, string countryCode)
    {
        customers = string.IsNullOrWhiteSpace(countryCode) ? customers : customers.Where(x => x.CountryCode == countryCode).ToList();




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


        return Task.FromResult(response);
    }
}