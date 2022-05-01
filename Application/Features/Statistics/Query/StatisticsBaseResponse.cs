namespace Application.Features.Statistics.Query;

public class StatisticsBaseResponse : BaseResponse
{
    public IReadOnlyList<CustomerDto> Customers { get; set; } = null!;
    public GetStatisticsQueryHandler.Statistic Overview { get; set; } = null!;
    public GetStatisticsQueryHandler.Statistic Sweden { get; set; } = null!;
    public GetStatisticsQueryHandler.Statistic Norway { get; set; } = null!;
    public GetStatisticsQueryHandler.Statistic Finland { get; set; } = null!;
}