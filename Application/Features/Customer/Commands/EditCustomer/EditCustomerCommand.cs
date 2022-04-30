namespace Application.Features.Customer.Commands.EditCustomer;
public class EditCustomerCommand : IRequest<EditCustomerResponse>
{
#pragma warning disable CS8618
    public CustomerDto Customer { get; set; }
#pragma warning restore CS8618
}