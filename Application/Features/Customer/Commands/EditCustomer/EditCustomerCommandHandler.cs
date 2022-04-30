using AzureSearch.Services;

namespace Application.Features.Customer.Commands.EditCustomer;

public class EditCustomerCommandHandler : IRequestHandler<EditCustomerCommand, EditCustomerResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly IAzureSearchService _azureSearchService;

    public EditCustomerCommandHandler(ApplicationDbContext context, IAzureSearchService azureSearchService)
    {
        _context = context;
        _azureSearchService = azureSearchService;
    }
    public async Task<EditCustomerResponse> Handle(EditCustomerCommand request, CancellationToken cancellationToken)
    {
        var response = new EditCustomerResponse();
        var validator = new EditCustomerValidator(_context);

        var result = await validator.ValidateAsync(request, cancellationToken);

        if (!result.IsValid)
        {
            response.Errors = result.Errors;
            response.Status = StatusCode.Error;
            response.StatusText = "Validation failed, check errors";

            return response;
        }



        try
        {
            var customerToUpdate = await _context.Customers.FindAsync(new object?[] { request.Customer.Id }, cancellationToken: cancellationToken);

            if (customerToUpdate == null) throw new Exception();


            _context.Entry(customerToUpdate).CurrentValues.SetValues(request.Customer);


            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            response.Status = StatusCode.Error;
            response.StatusText = "Could not save entity";
            return response;
        }



#pragma warning disable CS4014
        _azureSearchService.UploadDocuments();
#pragma warning restore CS4014
        return response;
    }
}