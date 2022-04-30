namespace Application.Features.Customer.Commands.CreateCustomer;
public class CreateCustomerCommand : IRequest<CreateCustomerResponse>
{
    public CustomerDto? Customer { get; init; }
}