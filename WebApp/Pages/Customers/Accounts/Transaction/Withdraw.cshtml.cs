using Application.Features.Account.Query.GetAccountDetailsForTransaction;
using Application.Features.Transactions.Commands.Withdraw;

namespace WebApp.Pages.Customers.Accounts.Transaction;

public class WithdrawModel : PageModel
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public WithdrawModel(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public string CustomerName { get; set; } = null!;
    public int CustomerId { get; set; }
    public AccountViewModel Account { get; private set; } = null!;
    public List<SelectListItem> Operations { get; set; } = null!;

    [PageRemote(
        ErrorMessage = "Balance is to low",
        AdditionalFields = "__RequestVerificationToken",
        HttpMethod = "post",
        PageHandler = "CheckBalance"
    )]
    [BindProperty]
    [Required(ErrorMessage = "Please enter amount")]
    [Range(0.01, 10000000, ErrorMessage = "{0} must be between $0.01 and $10,000,000.00")]
    public decimal? Amount { get; set; }


    [BindProperty]
    [Required(ErrorMessage = "Please enter operation")]
    public string? Operation { get; set; }



    public async Task OnGet(int customerId, int accountId)
    {
        PopulateDropdowns();
        await LoadProperties(customerId, accountId);
    }



    public async Task<IActionResult> OnPost(int customerId, int accountId)
    {
        var validOperations = new[] { "ATM withdrawal", "Payment" };
        PopulateDropdowns();
        await LoadProperties(customerId, accountId);

        if (validOperations.All(x => x != Operation)) ModelState.AddModelError(nameof(Operation), "Invalid Operation, nice try");
        if (!ModelState.IsValid) return Page();

        var response = await _mediator.Send(new WithdrawCommand() { Amount = Amount!.Value, Operation = Operation, AccountId = accountId });

        if (response.Status == Application.Responses.StatusCode.Error)
        {
            response.Errors?.ForEach(x => ModelState.AddModelError(x.PropertyName, x.ErrorMessage));
            return Page();
        }


        TempData["Message"] = $"Withdrew {Amount.Value:C} from account {accountId}";
        return Redirect($"/Customers/{customerId}/Accounts/{Account.Id}");
    }


    public async Task<JsonResult> OnPostCheckBalance(int customerId, int accountId)
    {
        await LoadProperties(customerId, accountId);
        var valid = Amount <= Account.Balance;
        return new JsonResult(valid);
    }

    private void PopulateDropdowns()
    {
        Operations = new List<SelectListItem>()
        {
            new()
            {
                Selected = true,
                Value = "",
                Text = "Choose operation..."
            },
            new()
            {
                Value = "ATM withdrawal",
                Text = "ATM withdrawal"
            },
            new()
            {
                Value = "Payment",
                Text = "Payment"
            }
        };
    }


    private async Task<IActionResult> LoadProperties(int customerId, int accountId)
    {
        var response =
            await _mediator.Send(new GetAccountDetailsForTransactionQuery() { CustomerId = customerId, AccountId = accountId });


        if (response.Status == Application.Responses.StatusCode.Error)
        {
            TempData["ErrorMessage"] = $"{response.StatusText}";
            return RedirectToPage("/PageNotFound");
        }

        CustomerName = response.CustomerName;
        CustomerId = response.CustomerId;
        Account = _mapper.Map<AccountViewModel>(response.Account);
        return Page();
    }

    public class AccountViewModel
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
    }
}