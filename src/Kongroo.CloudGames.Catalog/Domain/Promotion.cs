namespace Kongroo.CloudGames.Catalog.Domain;

public sealed record Promotion(PromotionId Id, Percentage Discount, DateTimeRange ActiveRange)
{
    public static Promotion Create(Percentage discount, DateTimeRange activeRange)
    {
        ArgumentNullException.ThrowIfNull(discount);
        ArgumentNullException.ThrowIfNull(activeRange);

        return new Promotion(PromotionId.Create(), discount, activeRange);
    }
}
