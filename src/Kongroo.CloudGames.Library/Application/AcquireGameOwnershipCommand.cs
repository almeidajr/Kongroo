namespace Kongroo.CloudGames.Library.Application;

public sealed record AcquireGameOwnershipCommand(Guid OwnerId, Guid GameId, Guid OrderId, DateTimeOffset AcquiredAt);
