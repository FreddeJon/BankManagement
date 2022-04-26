namespace Application.Features.Transactions.Commands.Transfer;

public class TransferValidator : AbstractValidator<TransferCommand>
{
    private readonly ApplicationDbContext _context;

    public TransferValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.FromAccountId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("FromAccountId is required")
            .MustAsync(Exist).WithMessage("Account you trying to transfer from does not exist");
        RuleFor(x => x.Amount)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Amount is required")
            .GreaterThan(0).LessThan(10000000000).WithMessage("{PropertyName} must be between $0.01 and $10,000,000.00")
            .MustAsync(EnoughBalance).WithMessage("Balance not enough");
        RuleFor(x => x.ToAccountId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("ToAccountId is required")
            .MustAsync(Exist).WithMessage("Account you trying to transfer to does not exist");

    }
    private async Task<bool> EnoughBalance(TransferCommand request, decimal amount, CancellationToken token)
    {
        var account = await _context.Accounts.FindAsync(new object?[] { request.FromAccountId }, cancellationToken: token);
        return account!.Balance >= amount;
    }
    private async Task<bool> Exist(TransferCommand request, int accountId, CancellationToken token)
    {
        return await _context.Accounts.FindAsync(new object?[] { accountId }, cancellationToken: token) is not null;
    }
}