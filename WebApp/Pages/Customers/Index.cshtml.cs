using Application.Features.AzureSearch.Query.GetCustomersUsingAzureSearchService;

namespace WebApp.Pages.Customers;

public class IndexModel : PageModel
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public IndexModel(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }


    [TempData]
    public string? Message { get; set; }

    public int TotalPages { get; set; }
    [BindProperty]

    public int CurrentPage { get; set; }
    [BindProperty]
    public string? CurrentCol { get; set; }
    [BindProperty]

    public string? CurrentOrder { get; set; }
    public string? Search { get; set; }
    public IReadOnlyList<CustomerViewModel> Customers { get; private set; }



    public async Task OnGet(int pageno, string? search, string? order, string? col)
    {
        var validCols = new[] { "Firstname", "Lastname", "Address", "City", "Country" };
        var validOrders = new[] { "asc", "desc" };

        CurrentPage = pageno > 1 ? pageno : 1;
        CurrentCol = validCols.Contains(col) ? col : "";
        CurrentOrder = validOrders.Contains(order) ? order : "asc";
        Search = search;


        var response = await _mediator.Send(new GetCustomersFromAzureSearchQuery() { Page = CurrentPage, Search = Search, SortColumn = CurrentCol, SortingOrder = CurrentOrder });


        TotalPages = response.TotalPage;

        CurrentPage = CurrentPage <= TotalPages ? CurrentPage : TotalPages;


        Customers = _mapper.Map<IReadOnlyList<CustomerViewModel>>(response.Customers);
    }

    public string SetSortIcon(string column)
    {
        if (CurrentCol != column) return string.Empty;

        return CurrentOrder == "desc" ? "fa-sort-up" : "fa-sort-down";
    }

    public string SetOrder(string column)
    {
        if (CurrentCol == column) return CurrentOrder == "asc" ? "desc" : "asc";

        return "asc";
    }
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string NationalId { get; set; } = null!;
    }
}