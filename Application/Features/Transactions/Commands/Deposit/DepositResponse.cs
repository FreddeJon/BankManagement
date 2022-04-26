using FluentValidation.Results;

namespace Application.Features.Transactions.Commands.Deposit;

public class DepositResponse : BaseResponse
{
    public List<ValidationFailure>? Errors { get; set; }
}