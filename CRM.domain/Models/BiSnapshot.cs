namespace CRM.domain.Models;

/// <summary>
/// Everything the BI form needs to render in one pass.
/// The form reads properties; it does not query.
/// </summary>
public class BiSnapshot
{
    // Row 1 KPIs
    public decimal TotalRevenue { get; set; }
    public int TotalTransactions { get; set; }
    public decimal TodaySales { get; set; }
    public int TodayOrders { get; set; }
    public int TotalOrders { get; set; }
    public decimal AverageOrderValue { get; set; }
    public int ActiveCustomers { get; set; }
    public int NewCustomersLast30Days { get; set; }
    public int TotalPoints { get; set; }

    // Row 2 KPIs
    public int LowStockCount { get; set; }
    public int OpenFeedbackCount { get; set; }
    public double AverageRating { get; set; }
    public int TotalFeedbackCount { get; set; }
    public int ActivePromotionsCount { get; set; }

    // Row 3 retention KPIs
    public int AtRiskCount { get; set; }
    public int DormantCount { get; set; }
    public int NeverOrderedCount { get; set; }
    public int ActiveForRetentionCount { get; set; }
    public decimal PointsLiability { get; set; }

    // Sales chart
    public List<BiSalesDay> SalesLast7 { get; set; } = new();

    // Top products
    public List<BiTopProduct> TopProducts { get; set; } = new();

    // Rating distribution (index 0 = 1 star ... index 4 = 5 stars)
    public int[] RatingDistribution { get; set; } = new int[5];

    // Payment split
    public List<BiPaymentSplit> PaymentSplit { get; set; } = new();

    // Retention split (four buckets, in order Active / At Risk / Dormant / Never)
    public List<BiRetentionSplit> RetentionSplit { get; set; } = new();

    // Retention recency (six buckets: 0-7 / 8-30 / 31-60 / 61-90 / 91-180 / 180+)
    public int[] RetentionRecency { get; set; } = new int[6];

    // Tables
    public List<BiRow> RecentOrders { get; set; } = new();
    public List<BiRow> TopCustomers { get; set; } = new();
    public List<BiRow> AtRiskList { get; set; } = new();

    // Insights
    public List<BiInsight> Insights { get; set; } = new();
}

public class BiSalesDay
{
    public DateTime Day { get; set; }
    public decimal Total { get; set; }
}

public class BiTopProduct
{
    public string Name { get; set; } = "";
    public int Qty { get; set; }
}

public class BiPaymentSplit
{
    public string Method { get; set; } = "";
    public int Count { get; set; }
}

public class BiRetentionSplit
{
    public string Status { get; set; } = "";
    public int Count { get; set; }
}

public class BiInsight
{
    public string Icon { get; set; } = "";
    /// <summary>"success" | "warning" | "danger" | "info" | "muted"</summary>
    public string Semantic { get; set; } = "info";
    public string Text { get; set; } = "";
}