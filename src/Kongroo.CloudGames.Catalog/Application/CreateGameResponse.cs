using System.ComponentModel;
using Kongroo.CloudGames.Catalog.Domain;

namespace Kongroo.CloudGames.Catalog.Application;

public sealed record CreateGameResponse(
    [property: Description("Unique identifier assigned to the created game.")] Guid Id,
    [property: Description("Display title of the game.")] string Title,
    [property: Description("Detailed summary of the game.")] string Description,
    [property: Description("Current game price amount.")] decimal PriceAmount,
    [property: Description("Current game price currency code.")] Currency Currency,
    [property: Description("Current publishing status of the game.")] GameStatus Status
);
