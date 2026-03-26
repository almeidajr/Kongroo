using Kongroo.SharedKernel;

namespace Kongroo.CloudGames.Catalog.Domain;

public class Game : Entity<GameId>
{
    private Game() { }

    public GameTitle Title { get; private set; } = null!;

    public GameDescription Description { get; private set; } = null!;

    public Money Price { get; private set; } = Money.Zero;

    public GameStatus Status { get; private set; }

    public static Game Create(GameTitle title, GameDescription description, Money price)
    {
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(description);
        ArgumentNullException.ThrowIfNull(price);

        var game = new Game
        {
            Id = GameId.Create(),
            Title = title,
            Description = description,
            Price = price,
            Status = GameStatus.Draft,
        };

        game.RaiseDomainEvent(new GameCreatedDomainEvent(game.Id));

        return game;
    }

    public void ChangeDetails(GameTitle title, GameDescription description)
    {
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(description);

        if (Title == title && Description == description)
        {
            return;
        }

        var previousTitle = Title;
        var previousDescription = Description;
        Title = title;
        Description = description;
        RaiseDomainEvent(new GameDetailsChangedDomainEvent(Id, previousTitle, Title, previousDescription, Description));
    }

    public void ChangePrice(Money price)
    {
        ArgumentNullException.ThrowIfNull(price);

        if (Price == price)
        {
            return;
        }

        var previousPrice = Price;
        Price = price;
        RaiseDomainEvent(new GamePriceChangedDomainEvent(Id, previousPrice, Price));
    }

    public void ChangeStatus(GameStatus status)
    {
        if (Status == status)
        {
            return;
        }

        var previousStatus = Status;
        Status = status;
        RaiseDomainEvent(new GameStatusChangedDomainEvent(Id, previousStatus, Status));
    }
}
