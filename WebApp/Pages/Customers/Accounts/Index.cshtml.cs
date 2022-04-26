using Application.Features.Account.Query.GetAccountDetails;
using Application.Features.Transactions.Query.GetTransactionsForAccountPaginated;

namespace WebApp.Pages.Customers.Accounts;

public class IndexModel : PageModel
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public IndexModel(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public AccountViewModel Account { get; set; } = null!;
    public CustomerViewModel Customer { get; set; } = null!;
    public int TotalPage { get; set; }
    public int TotalTransactions { get; set; }


    public async Task<IActionResult> OnGet(int customerId, int accountId)
    {
        var response = await _mediator.Send(new GetAccountAndCustomerQuery() { AccountId = accountId, CustomerId = customerId });

        if (response.Status == Application.Responses.StatusCode.Error) return RedirectToPage("404");

        TotalPage = response.TotalPages;
        TotalTransactions = response.TotalTransactions;
        Customer = _mapper.Map<CustomerViewModel>(response.Customer);
        Account = _mapper.Map<AccountViewModel>(response.Account);


        return Page();
    }



    public async Task<IActionResult> OnGetLoadTransactions(int accountId, int pageNo)
    {
        var response = await _mediator.Send(new GetTransactionsQuery() { AccountId = accountId, Page = pageNo });

        if (response.Status == Application.Responses.StatusCode.Error) return new JsonResult(null);


        return new JsonResult(new { items = _mapper.Map<List<TransactionViewModel>>(response.Transactions), lastPage = response.TotalPages });
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
    }
    public class AccountViewModel
    {
        public int Id { get; init; }
        public string AccountType { get; init; } = null!;
        public DateTime Created { get; init; }
        public decimal Balance { get; init; }
        // ReSharper disable once CollectionNeverUpdated.Global
        public List<TransactionViewModel> Transactions { get; init; } = new();
    }
    public class TransactionViewModel
    {
        public int Id { get; init; }
        public string Type { get; init; } = null!;
        public string Operation { get; init; } = null!;
        public DateTime Date { get; init; }
        public string Amount { get; init; } = null!;
        public string NewBalance { get; init; } = null!;
    }
}

