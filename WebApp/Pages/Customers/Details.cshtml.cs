using Application.Features.Customer.Query.GetCustomerByIdIncludeAccounts;

namespace WebApp.Pages.Customers;

public class DetailsModel : PageModel
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CustomerViewModel? Customer { get; private set; }

    public DetailsModel(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }


    public async Task<IActionResult> OnGet(int customerId)
    {
        var response = await _mediator.Send(new GetCustomerQuery() { CustomerId = customerId });

        if (response.Status == Application.Responses.StatusCode.Error)
        {
            return RedirectToPage("404");
        }

        Customer = _mapper.Map<CustomerViewModel>(response.Customer);
        return Page();
    }

    public class CustomerViewModel
    {
        public int Id { get; set; }
        public string NationalId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Streetaddress { get; set; } = null!;
        public string Telephone { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string Zipcode { get; set; } = null!;
        public decimal Balance { get; set; }
        public List<AccountViewModel> Accounts { get; set; } = new();
    }

    public class AccountViewModel
    {
        public int Id { get; set; }
        public string AccountType { get; set; } = null!;
        public decimal Balance { get; set; }
        public DateTime? LatestTransaction { get; set; }
    }
}