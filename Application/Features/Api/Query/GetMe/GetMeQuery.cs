namespace Application.Features.Api.Query.GetMe;
public class GetMeQuery : IRequest<GetMeResponse>
{
    public string UserId { get; set; }
}