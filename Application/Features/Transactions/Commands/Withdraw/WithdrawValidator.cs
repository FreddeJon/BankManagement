namespace Application.Features.Transactions.Commands.Withdraw;

public class WithdrawValidator : AbstractValidator<WithdrawCommand>
{
    private readonly ApplicationDbContext _context;

    public WithdrawValidator(ApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.AccountId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MustAsync(Exist).WithMessage("Account not found");
        RuleFor(x => x.Amount)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .GreaterThan(0).LessThan(10000000000).WithMessage("{PropertyName} must be between $0.01 and $10,000,000.00")
            .MustAsync(EnoughBalance).WithMessage("Balance not enough");
        RuleFor(x => x.Operation)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().NotEmpty().WithMessage("{PropertyName} is required")
            .Must(ValidOperation).WithMessage("{PropertyName} is not a valid operation");


    }
    private async Task<bool> EnoughBalance(WithdrawCommand request, decimal amount, CancellationToken token)
    {
        var account = await _context.Accounts.FindAsync(request.AccountId);
        return account!.Balance >= amount;
    }
    private async Task<bool> Exist(int accountId, CancellationToken token)
    {
        return (await _context.Accounts.FindAsync(accountId) is not null);
    }


    private static bool ValidOperation(string? operation)
    {
        var validOperations = new[] { "ATM withdrawal", "Payment" };
        return validOperations.Any(x => x == operation);
    }

}