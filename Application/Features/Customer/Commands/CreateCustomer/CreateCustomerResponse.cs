using FluentValidation.Results;

namespace Application.Features.Customer.Commands.CreateCustomer;

public class CreateCustomerResponse : BaseResponse
{
    public List<ValidationFailure>? Errors { get; set; }
    public int CustomerId { get; set; }
}