using Application.Features.Statistics.Query;

namespace WebApp.Pages;

[ResponseCache(Duration = 60, VaryByQueryKeys = new[] {"countrycode"})]
public class IndexModel : PageModel
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    public IndexModel(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }
    public IReadOnlyList<CustomerViewModel> Customers { get; set; }

    public Statistic Finland { get; set; } = null!;

    public Statistic Norway { get; set; } = null!;

    public Statistic Overview { get; set; } = null!;

    public Statistic Sweden { get; set; } = null!;


    public async Task<IActionResult> OnGet(string? countryCode)
    {
        var response = await _mediator.Send(new GetStatisticsQuery() { CountryCode = countryCode });

        if (response.Status == Application.Responses.StatusCode.Error) return RedirectToPage("Error");


        Customers = _mapper.Map<IReadOnlyList<CustomerViewModel>>(response.Customers);
        Overview = response.Overview;
        Sweden = response.Sweden;
        Norway = response.Norway;
        Finland = response.Finland;
        return Page();
    }



    public class CustomerViewModel
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public string Country { get; init; } = null!;
        public string City { get; init; } = null!;
        public string Streetaddress { get; init; } = null!;
        public string Balance { get; init; }
    }
}