using FluentValidation.Results;

namespace Application.Features.Transactions.Commands.Transfer;

public class TransferResponse : BaseResponse
{
    public List<ValidationFailure>? Errors { get; set; }
}