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


        var statistics = await _context.Customers.Include(x => x.Accounts)
            .SelectMany(x => x.Accounts, (customer, account) => new { customer.CountryCode, account })
            .GroupBy(c => c.CountryCode).Select(x =>
                new Statistic()
                {
                    CountryCode = x.Key,
                    TotalCustomers = _context.Customers.Count(c => c.CountryCode == x.Key),
                    TotalBalance = x.Sum(s => s.account.Balance),
                    TotalAccounts = x.Count()
                }).ToListAsync(cancellationToken: cancellationToken);

        statistics.ForEach(x => x.Country = GetCountry(x.CountryCode));

        var overview = new Statistic
        {
            TotalBalance = statistics.Sum(x => x.TotalBalance),
            TotalAccounts = statistics.Sum(x => x.TotalAccounts),
            TotalCustomers = statistics.Sum(x => x.TotalCustomers),
            CountryCode = "",
            Country = "Overview"
        };


        statistics.ForEach(x => x.Percentage = SetPercentage(x.TotalCustomers, overview.TotalCustomers));




        response.Overview = overview;
        response.Sweden = statistics.FirstOrDefault(x => x.CountryCode == "SE");
        response.Finland = statistics.FirstOrDefault(x => x.CountryCode == "FI");
        response.Norway = statistics.FirstOrDefault(x => x.CountryCode == "NO");





        var validCountryCodes = new[] { "SE", "FI", "NO" };



        var query = _context.Customers.Include(x => x.Accounts).AsQueryable();

        if (request.CountryCode != null && validCountryCodes.Any(x => x.Contains(request.CountryCode.ToUpper())))
        {
            query = query.Where(x => x.CountryCode == request.CountryCode);
        }


        response.Customers = _mapper.Map<IReadOnlyList<CustomerDto>>(await query
            .OrderByDescending(x => x.Accounts.Sum(b => b.Balance)).Take(10).ToListAsync(cancellationToken: cancellationToken));

        return response;

    }




    public class Statistic
    {
        public string? Country { get; set; }
        public string CountryCode { get; set; } = null!;
        public int TotalAccounts { get; init; }
        public int TotalCustomers { get; init; }
        public decimal TotalBalance { get; init; }
        public double Percentage { get; set; }
    }
    private static double SetPercentage(int objCustomerCount, int overviewCustomerCount)
    {
        return (double)objCustomerCount / overviewCustomerCount;
    }
    private static string? GetCountry(string countryCode)
    {
        return countryCode switch
        {
            "SE" => "Sweden",
            "NO" => "Norway",
            "FI" => "Finland",
            _ => "Overview"
        };
    }
}