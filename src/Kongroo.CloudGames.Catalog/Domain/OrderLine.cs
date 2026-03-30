namespace Kongroo.CloudGames.Catalog.Domain;

public sealed record OrderLine(
    GameId GameId,
    GameTitle GameTitle,
    Money ListPrice,
    Money FinalPrice,
    PromotionId? AppliedPromotionId
)
{
    public static OrderLine FromQuote(GamePurchaseQuote quote)
    {
        ArgumentNullException.ThrowIfNull(quote);

        return new OrderLine(
            quote.GameId,
            quote.GameTitle,
            quote.ListPrice,
            quote.FinalPrice,
            quote.AppliedPromotionId
        );
    }
}
