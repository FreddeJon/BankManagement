using FluentValidation.Results;

namespace Application.Features.Customer.Commands.EditCustomer;

public class EditCustomerResponse : BaseResponse
{
    public List<ValidationFailure>? Errors { get; set; }
}