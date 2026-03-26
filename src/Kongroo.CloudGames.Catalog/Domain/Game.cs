using Kongroo.SharedKernel;

namespace Kongroo.CloudGames.Catalog.Domain;

public class Game : Entity<GameId>
{
    private Game() { }

    public required GameTitle Title { get; init; }

    public required GameDescription Description { get; init; }

    public Money Price { get; private set; } = Money.Zero;

    public GameStatus Status { get; private set; }

    public static Game Create(GameTitle title, GameDescription description, Money price)
    {
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

    public void ChangePrice(Money price)
    {
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
