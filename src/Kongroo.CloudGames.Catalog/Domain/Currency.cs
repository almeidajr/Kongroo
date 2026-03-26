namespace Kongroo.CloudGames.Catalog.Domain;

public record Currency(string Code)
{
    public static readonly Currency Brl = new("BRL");
    public static readonly Currency Eur = new("EUR");
    public static readonly Currency Usd = new("USD");

    public static Currency From(string code) =>
        code switch
        {
            "BRL" => Brl,
            "EUR" => Eur,
            "USD" => Usd,
            _ => throw new ArgumentException($"Unsupported currency: {code}", nameof(code)),
        };
}
