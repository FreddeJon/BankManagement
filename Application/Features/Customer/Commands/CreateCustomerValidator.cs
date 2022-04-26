namespace Application.Features.Customer.Commands;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
{
    private readonly ApplicationDbContext _context;

    public CreateCustomerValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Customer)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("{PropertyName} is required")
            .MustAsync(NotFound).WithMessage("Customer already exists");
        RuleFor(x => x.Customer!.Givenname)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(50).WithMessage("{PropertyName} maximum length {MaxLength}");
        RuleFor(x => x.Customer!.Surname)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(50).WithMessage("{PropertyName} maximum length {MaxLength}");
        RuleFor(x => x.Customer!.Zipcode)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(10).WithMessage("{PropertyName} maximum length {MaxLength}");
        RuleFor(x => x.Customer!.Streetaddress)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(50).WithMessage("{PropertyName} maximum length {MaxLength}");
        RuleFor(x => x.Customer!.City)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(50).WithMessage("{PropertyName} maximum length {MaxLength}");
        RuleFor(x => x.Customer!.Country)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(30).WithMessage("{PropertyName} maximum length {MaxLength}");
        RuleFor(x => x.Customer!.NationalId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(20).WithMessage("{PropertyName} maximum length {MaxLength}");
        RuleFor(x => x.Customer!.Telephone)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(50).WithMessage("{PropertyName} maximum length {MaxLength}");
        RuleFor(x => x.Customer!.EmailAddress)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(50).WithMessage("{PropertyName} maximum length {MaxLength}");
        RuleFor(x => x.Customer!.Birthday)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .LessThan(DateTime.Now).WithMessage("{PropertyName} invalid");
    }

    private async Task<bool> NotFound(CreateCustomerCommand request, CustomerDto? customer, CancellationToken token)
    {
        return await _context.Customers.AllAsync(x =>
            x.EmailAddress != customer!.EmailAddress && x.NationalId != customer.NationalId, cancellationToken: token);
    }
}