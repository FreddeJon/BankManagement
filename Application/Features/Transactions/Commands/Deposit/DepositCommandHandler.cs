namespace Application.Features.Transactions.Commands.Deposit;

public class DepositCommandHandler : IRequestHandler<DepositCommand, DepositResponse>
{

    private readonly ApplicationDbContext _context;

    public DepositCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<DepositResponse> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        var response = new DepositResponse();
        var validator = new DepositValidator(_context);
        var result = await validator.ValidateAsync(request, cancellationToken);

        if (!result.IsValid)
        {
            response.Errors = result.Errors;
            response.Status = StatusCode.Error;
            response.StatusText = "Validation failed, check errors!";

            return response;
        }

        var account = await _context.Accounts.FindAsync(new object?[] { request.AccountId }, cancellationToken: cancellationToken);

        var transaction = new Transaction()
        {
            Amount = request.Amount,
            Operation = request.Operation,
            Type = "Debit",
            NewBalance = account!.Balance + request.Amount,
            Date = DateTime.Now
        };

        account.Balance = transaction.NewBalance;
        account.Transactions.Add(transaction);
        await _context.SaveChangesAsync(cancellationToken);

        return response;
    }
}