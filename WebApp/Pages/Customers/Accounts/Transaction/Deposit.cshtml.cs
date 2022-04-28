using Application.Features.Account.Query.GetAccountDetailsForTransaction;
using Application.Features.Transactions.Commands.Deposit;

namespace WebApp.Pages.Customers.Accounts.Transaction;

[AutoValidateAntiforgeryToken]
public class DepositModel : PageModel
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

#pragma warning disable CS8618
    public DepositModel(IMapper mapper, IMediator mediator)
#pragma warning restore CS8618
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public AccountViewModel Account { get; set; } = null!;
    public List<SelectListItem> Operations { get; set; }
    public string CustomerName { get; set; }
    public int CustomerId { get; set; }



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
        var validOperations = new[] { "Salary", "Deposit cash" };
        PopulateDropdowns();
        await LoadProperties(customerId, accountId);

        if (validOperations.All(x => x != Operation)) ModelState.AddModelError(nameof(Operation), "Invalid Operation, nice try");
        if (!ModelState.IsValid) return Page();

        var response =
           await _mediator.Send(new DepositCommand() { Amount = Amount!.Value, AccountId = accountId, Operation = Operation! });


        if (response.Status == Application.Responses.StatusCode.Error)
        {
            response.Errors?.ForEach(x => ModelState.AddModelError(x.PropertyName, x.ErrorMessage));
            return Page();
        }

        TempData["Message"] = $"Deposited {Amount.Value:C} to account {accountId}";
        return Redirect($"/Customers/{customerId}/Accounts/{Account.Id}");
    }


    public void PopulateDropdowns()
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
                Value = "Deposit cash",
                Text = "Deposit cash"
            },
            new()
            {
                Value = "Salary",
                Text = "Salary"
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