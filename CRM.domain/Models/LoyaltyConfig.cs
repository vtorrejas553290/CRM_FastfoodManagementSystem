namespace CRM.domain.Models;

/// <summary>
/// Central loyalty rate configuration.
/// Changing the earn rate here changes it everywhere.
/// </summary>
public static class LoyaltyConfig
{
    /// <summary>
    /// Points earned per 1 peso spent.
    /// 10 pts/₱1 = a 10% cashback at the 100 pts = ₱1 redemption rate.
    /// </summary>
    public const decimal PointsPerPeso = 10m;

    /// <summary>
    /// Points required to redeem ₱1 of discount.
    /// 100 pts = ₱1. Combined with PointsPerPeso = 10, this yields a 10% earn rate.
    /// </summary>
    public const int PointsPerPesoDiscount = 100;

    /// <summary>
    /// Points a customer earns for spending <paramref name="amount"/> pesos.
    /// Always rounds down.
    /// </summary>
    public static int PointsEarnedFor(decimal amount)
        => (int)Math.Floor(amount * PointsPerPeso);

    /// <summary>
    /// Peso value of a given number of points at the redemption rate.
    /// </summary>
    public static decimal PesosForPoints(int points)
        => points / (decimal)PointsPerPesoDiscount;

    /// <summary>
    /// Human-readable description of the rate, used in info labels.
    /// </summary>
    public static string RateDescription
        => $"₱1 spent = {PointsPerPeso:N0} points earned. {PointsPerPesoDiscount:N0} points = ₱1 discount.";
}