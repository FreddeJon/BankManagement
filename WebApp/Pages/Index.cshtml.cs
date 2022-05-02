using Application.Features.Statistics.Query;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages;

[ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "countrycode" })]
public class IndexModel : PageModel
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _context;

    public IndexModel(IMapper mapper, IMediator mediator, ApplicationDbContext context)
    {
        _mapper = mapper;
        _mediator = mediator;
        _context = context;
    }
    public IReadOnlyList<CustomerViewModel>? Customers { get; set; }

    public GetStatisticsQueryHandler.Statistic Finland { get; set; } = null!;

    public GetStatisticsQueryHandler.Statistic Norway { get; set; } = null!;

    public GetStatisticsQueryHandler.Statistic Overview { get; set; } = null!;

    public GetStatisticsQueryHandler.Statistic Sweden { get; set; } = null!;


    public async Task<IActionResult> OnGet(string? countryCode)
    {
        var response = await _mediator.Send(new GetStatisticsQuery() { CountryCode = countryCode });

        if (response.Status == Application.Responses.StatusCode.Error)
        {
            TempData["ErrorMessage"] = $"{response.StatusText}";
            return RedirectToPage("/PageNotFound");
        }


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
        public string Balance { get; init; } = null!;
    }
}