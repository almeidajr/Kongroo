using Kongroo.BuildingBlocks.Application;
using Kongroo.CloudGames.Catalog.Domain;
using Microsoft.Extensions.Logging;

namespace Kongroo.CloudGames.Catalog.Application;

public sealed class OrderPlacedDomainEventHandler(ILogger<OrderPlacedDomainEventHandler> logger)
    : DomainEventHandler<OrderPlacedDomainEvent>
{
    public override Task HandleAsync(OrderPlacedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Order placed. OrderId: {OrderId}, BuyerId: {BuyerId}, TotalAmount: {TotalAmount}, Currency: {Currency}",
            domainEvent.OrderId.Value,
            domainEvent.BuyerId.Value,
            domainEvent.Total.Amount,
            domainEvent.Total.Currency
        );

        return Task.CompletedTask;
    }
}
