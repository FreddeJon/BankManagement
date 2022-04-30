namespace Application.Features.Customer.Commands.EditCustomer;

public class EditCustomerValidator : AbstractValidator<EditCustomerCommand>
{
    private readonly ApplicationDbContext _context;

    public EditCustomerValidator(ApplicationDbContext context)
    {
        _context = context;


        RuleFor(x => x.Customer.Id)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("{PropertyName} is required")
            .MustAsync(CustomerExists).WithMessage("Customer not found");
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

    private async Task<bool> CustomerExists(EditCustomerCommand request, int id, CancellationToken token)
    {
        return await _context.Customers.AnyAsync(x => x.Id == id, cancellationToken: token);
    }

    private async Task<bool> EmailNotFound(EditCustomerCommand request, string email, CancellationToken token)
    {
        var currentCustomer = await _context.Customers.FindAsync(request.Customer.Id);

        if (currentCustomer is null) return false;

        if (currentCustomer.EmailAddress != request.Customer.EmailAddress)
        {

            return await _context.Customers.AllAsync(x =>
                x.EmailAddress != request.Customer.EmailAddress, cancellationToken: token);
        }

        return true;
    }




    private async Task<bool> NationalIdNotFound(EditCustomerCommand request, string nationalId, CancellationToken token)
    {
        var currentCustomer = await _context.Customers.FindAsync(request.Customer.Id);

        if (currentCustomer is null) return false;

        if (currentCustomer.NationalId != request.Customer.NationalId)
        {

            return await _context.Customers.AllAsync(x =>
                x.NationalId != request.Customer.NationalId, cancellationToken: token);
        }

        return true;
    }

}