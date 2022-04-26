namespace Application.Features.Customer.Commands;
public class CreateCustomerCommand : IRequest<CreateCustomerResponse>
{
    public CustomerDto? Customer { get; init; }
}