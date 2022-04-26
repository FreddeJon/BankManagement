using FluentValidation.Results;

namespace Application.Features.Transactions.Commands.Withdraw;

public class WithdrawResponse : BaseResponse
{
    public List<ValidationFailure>? Errors { get; set; }
}