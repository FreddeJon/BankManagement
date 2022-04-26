namespace Application.Features.Transactions.Commands.Transfer;

public class TransferCommandHandler : IRequestHandler<TransferCommand, TransferResponse>
{
    private readonly ApplicationDbContext _context;

    public TransferCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<TransferResponse> Handle(TransferCommand request, CancellationToken cancellationToken)
    {
        var response = new TransferResponse();
        var validator = new TransferValidator(_context);

        var result = await validator.ValidateAsync(request, cancellationToken);
        if (!result.IsValid)
        {
            response.Errors = result.Errors;
            response.Status = StatusCode.Error;
            response.StatusText = "Validation failed, check errors";
            return response;
        }


        var fromAccount = await _context.Accounts.FindAsync(new object?[] { request.FromAccountId }, cancellationToken: cancellationToken);
        var toAccount = await _context.Accounts.FindAsync(new object?[] { request.ToAccountId }, cancellationToken: cancellationToken);

        var fromAccountTransaction = new Transaction() { Amount = request.Amount, Operation = "Transfer", Type = "Credit", Date = DateTime.Now, NewBalance = fromAccount!.Balance -= request.Amount };
        fromAccount.Transactions.Add(fromAccountTransaction);
        fromAccount.Balance = fromAccountTransaction.NewBalance;

        var toAccountTransaction = new Transaction() { Amount = request.Amount, Operation = "Transfer", Type = "Debit", Date = DateTime.Now, NewBalance = toAccount!.Balance += request.Amount };
        toAccount.Transactions.Add(toAccountTransaction);
        toAccount.Balance = toAccountTransaction.NewBalance;
        await _context.SaveChangesAsync(cancellationToken);


        return response;
    }
}