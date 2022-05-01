namespace Application.Features.Customer.Commands.CreateCustomer;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
{
    private readonly ApplicationDbContext _context;

    public CreateCustomerValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Customer)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("{PropertyName} is required");
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
            .MustAsync(NationalIdNotFound).WithMessage("National Id already in use")
            .MaximumLength(20).WithMessage("{PropertyName} maximum length {MaxLength}");
        RuleFor(x => x.Customer!.Telephone)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(50).WithMessage("{PropertyName} maximum length {MaxLength}");
        RuleFor(x => x.Customer!.EmailAddress)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MustAsync(EmailNotFound).WithMessage("Email already in use")
            .MaximumLength(50).WithMessage("{PropertyName} maximum length {MaxLength}");
        RuleFor(x => x.Customer!.Birthday)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .LessThan(DateTime.Now).WithMessage("{PropertyName} invalid");
    }


    private async Task<bool> EmailNotFound(CreateCustomerCommand request, string email, CancellationToken token)
    {
        return await _context.Customers.AllAsync(x => x.EmailAddress != request.Customer!.EmailAddress, cancellationToken: token);
    }




    private async Task<bool> NationalIdNotFound(CreateCustomerCommand request, string nationalId, CancellationToken token)
    {
        return await _context.Customers.AllAsync(x => x.NationalId != request.Customer!.NationalId, cancellationToken: token);
    }
}