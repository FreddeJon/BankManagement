namespace Application.Features.Transactions.Commands.Withdraw;

public class WithdrawCommandHandler : IRequestHandler<WithdrawCommand, WithdrawResponse>
{
    private readonly ApplicationDbContext _context;

    public WithdrawCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<WithdrawResponse> Handle(WithdrawCommand request, CancellationToken cancellationToken)
    {
        var response = new WithdrawResponse();
        var validator = new WithdrawValidator(_context);
        var result = await validator.ValidateAsync(request, cancellationToken);

        if (!result.IsValid)
        {
            response.Errors = result.Errors;
            response.Status = StatusCode.Error;
            response.StatusText = "Validation failed, check errors!";

            return response;
        }



        var account = await _context.Accounts.FindAsync(request.AccountId);

        if (account != null)
        {
            var transaction = new Transaction()
            {
                Amount = request.Amount,
                Operation = request.Operation,
                Type = "Credit",
                NewBalance = account.Balance - request.Amount,
                Date = DateTime.UtcNow
            };

            account.Balance = transaction.NewBalance;
            account.Transactions.Add(transaction);
        }

        await _context.SaveChangesAsync(cancellationToken);


        return response;
    }
}