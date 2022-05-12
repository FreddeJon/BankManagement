namespace Application.Features.Api.Query.GetMe;

public class GetMeResponse : BaseResponse
{
    public CustomerDto Customer { get; set; }
}