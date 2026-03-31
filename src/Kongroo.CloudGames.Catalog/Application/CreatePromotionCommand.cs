namespace Kongroo.CloudGames.Catalog.Application;

public sealed record CreatePromotionCommand(
    Guid GameId,
    decimal Discount,
    DateTimeOffset StartsAt,
    DateTimeOffset EndsAt
);
