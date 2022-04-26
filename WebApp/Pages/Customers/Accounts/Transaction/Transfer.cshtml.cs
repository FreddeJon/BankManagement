using Application.Features.Account.Query.GetAccountDetailsForTransaction;
using Application.Features.Transactions.Commands.Transfer;

namespace WebApp.Pages.Customers.Accounts.Transaction;

public class TransferModel : PageModel
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public TransferModel(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public string CustomerName { get; set; } = null!;
    public int CustomerId { get; set; }
    public AccountViewModel AccountToTransferFrom { get; private set; } = null!;


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
    [Required(ErrorMessage = "Please enter account to transfer to")]
    public int? ToAccountId { get; set; }




    public async Task OnGet(int customerId, int accountId)
    {
        await LoadProperties(customerId, accountId);
    }



    public async Task<IActionResult> OnPost(int customerId, int accountId)
    {
        await LoadProperties(customerId, accountId);

        if (!ModelState.IsValid) return Page();

        var response = await _mediator.Send(new TransferCommand() { Amount = Amount!.Value, FromAccountId = AccountToTransferFrom.Id, ToAccountId = (int)ToAccountId! });

        if (response.Status == Application.Responses.StatusCode.Error)
        {
            response.Errors?.ForEach(x => ModelState.AddModelError(x.PropertyName, x.ErrorMessage));
            return Page();
        }

        return Redirect($"/Customers/{customerId}/Accounts/{AccountToTransferFrom.Id}");
    }


    public async Task<JsonResult> OnPostCheckBalance(int customerId, int accountId)
    {
        await LoadProperties(customerId, accountId);
        var valid = Amount <= AccountToTransferFrom.Balance;
        return new JsonResult(valid);
    }


    private async Task<IActionResult> LoadProperties(int customerId, int accountId)
    {
        var response =
            await _mediator.Send(new GetAccountDetailsForTransactionQuery() { CustomerId = customerId, AccountId = accountId });


        if (response.Status == Application.Responses.StatusCode.Error)
        {
            return RedirectToPage("404");
        }

        CustomerName = response.CustomerName;
        CustomerId = response.CustomerId;
        AccountToTransferFrom = _mapper.Map<AccountViewModel>(response.Account);
        return Page();
    }

    public class AccountViewModel
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
    }

}