namespace Application.Responses;
public class BaseResponse
{
    protected BaseResponse()
    {
        Status = StatusCode.Success;
        StatusText = "Success";
    }
    public string StatusText { get; set; }
    public StatusCode Status { get; set; }

}

public enum StatusCode
{
    Success,
    Error
}
