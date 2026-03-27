using Kongroo.CloudGames.Catalog.Domain;

namespace Kongroo.CloudGames.Catalog.Application;

public sealed record UpdateGameCommand(
    Guid GameId,
    string Title,
    string Description,
    decimal PriceAmount,
    Currency Currency,
    GameStatus Status
);
