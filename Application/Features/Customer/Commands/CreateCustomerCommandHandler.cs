namespace Application.Features.Customer.Commands;
public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CreateCustomerResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateCustomerCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<CreateCustomerResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var response = new CreateCustomerResponse();
        var validator = new CreateCustomerValidator(_context);

        var result = await validator.ValidateAsync(request, cancellationToken);
        if (!result.IsValid)
        {
            response.Errors = result.Errors;
            response.StatusText = "Validation failed, check errors";
            response.Status = StatusCode.Error;
            return response;
        }

        var customer = _mapper.Map<Domain.Entities.Customer>(request.Customer);
        customer.Accounts.Add(new Domain.Entities.Account() { AccountType = "Personal", Balance = 0, Created = DateTime.Now, Transactions = new List<Transaction>() });
        await _context.Customers.AddAsync(customer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        response.CustomerId = customer.Id;
        return response;
    }
}