using AzureSearch.Services;
using Microsoft.AspNetCore.Identity;
using Persistence.Data;
using Persistence.Models;

namespace Application.Features.Customer.Commands.CreateCustomer;
public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CreateCustomerResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IAzureSearchService _azureSearchService;
    private readonly UserManager<IdentityUser> _userManager;

    public CreateCustomerCommandHandler(ApplicationDbContext context, IMapper mapper, IAzureSearchService azureSearchService, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _mapper = mapper;
        _azureSearchService = azureSearchService;
        _userManager = userManager;
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
        var password = $"{request.Customer?.Givenname}{request.Customer?.Surname}1".ToLower();
        var user = await _userManager.SeedUserAsync(request.Customer?.EmailAddress.ToLower(), password, new[] { nameof(ApplicationRoles.Customer) });
        _context.CustomerUser.Add(new CustomerUser() { CustomerId = customer.Id, UserId = user!.Id });
        _context.SaveChanges();



#pragma warning disable CS4014
        _azureSearchService.UploadDocuments();
#pragma warning restore CS4014

        return response;
    }


}