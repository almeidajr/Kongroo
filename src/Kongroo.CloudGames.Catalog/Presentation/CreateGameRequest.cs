using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Kongroo.CloudGames.Catalog.Domain;
using Kongroo.SharedKernel.Attributes;

namespace Kongroo.CloudGames.Catalog.Presentation;

public sealed record CreateGameRequest(
    [property: Required]
    [property: MinLength(GameTitle.MinLength)]
    [property: MaxLength(GameTitle.MaxLength)]
    [property: Description("Display title of the game.")]
        string Title,
    [property: Required]
    [property: MinLength(GameDescription.MinLength)]
    [property: MaxLength(GameDescription.MaxLength)]
    [property: Description("Detailed summary of the game.")]
        string Description,
    [property: Required]
    [property: NonNegative<decimal>]
    [property: Description("Current game price amount.")]
        decimal PriceAmount,
    [property: Description("Current game price currency code.")] Currency Currency
);
