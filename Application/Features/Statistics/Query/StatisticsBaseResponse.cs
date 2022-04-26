namespace Application.Features.Statistics.Query;

public class StatisticsBaseResponse : BaseResponse
{
    public IReadOnlyList<CustomerDto> Customers { get; set; } = null!;
    public Statistic Overview { get; set; } = null!;
    public Statistic Sweden { get; set; } = null!;
    public Statistic Norway { get; set; } = null!;
    public Statistic Finland { get; set; } = null!;
}