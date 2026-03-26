namespace Kongroo.CloudGames.Catalog.Application;

public sealed record CreateGameCommand(string Title, string Description, decimal PriceAmount, string Currency);
