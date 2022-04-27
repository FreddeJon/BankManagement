namespace Application.Features.Transactions.Commands.Deposit;
public class DepositValidator : AbstractValidator<DepositCommand>
{
    private readonly ApplicationDbContext _context;

    public DepositValidator(ApplicationDbContext context)
    {
        _context = context;
        RuleFor(x => x.AccountId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MustAsync(Exist).WithMessage("Account not found");
        RuleFor(x => x.Amount)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .GreaterThan(0).LessThanOrEqualTo(10000000)
            .WithMessage("{PropertyName} must be between $0.01 and $10,000,000.00");
        RuleFor(x => x.Operation)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().NotEmpty().WithMessage("{PropertyName} is required")
            .Must(ValidOperation).WithMessage("{PropertyName} is not a valid operation");


    }

    private async Task<bool> Exist(int accountId, CancellationToken token)
    {
        return (await _context.Accounts.FindAsync(new object?[] { accountId }, cancellationToken: token) is not null);
    }


    private static bool ValidOperation(string? operation)
    {
        var validOperations = new[] { "Salary", "Deposit cash" };
        return validOperations.Any(x => x == operation);
    }

}