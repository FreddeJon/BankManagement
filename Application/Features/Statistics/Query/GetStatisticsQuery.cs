namespace Application.Features.Statistics.Query;
public class GetStatisticsQuery : IRequest<StatisticsBaseResponse>
{
    public string? CountryCode { get; init; }
}