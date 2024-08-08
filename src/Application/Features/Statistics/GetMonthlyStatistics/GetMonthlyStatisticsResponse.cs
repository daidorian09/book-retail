namespace Application.Features.Statistics.GetMonthlyStatistics;

public class GetMonthlyStatisticsQueryResponse
{
    public string Month { get; set; }
    public long TotalOrderCount { get; set; }
    public long TotalPurchasedBooks { get; set; }
    public decimal MonthlySum { get; set; }
}
