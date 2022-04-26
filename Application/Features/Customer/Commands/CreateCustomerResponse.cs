using FluentValidation.Results;

namespace Application.Features.Customer.Commands;

public class CreateCustomerResponse : BaseResponse
{
    public List<ValidationFailure>? Errors { get; set; }
    public int CustomerId { get; set; }
}