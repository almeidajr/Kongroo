using Kongroo.SharedKernel;

namespace Kongroo.CloudGames.Catalog.Domain;

public class Game : Entity<GameId>
{
    public static Game Create()
    {
        var game = new Game { Id = GameId.Create() };
        game.RaiseDomainEvent(new GameCreatedDomainEvent(game.Id));
        return game;
    }
}
