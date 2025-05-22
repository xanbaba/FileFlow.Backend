using MediatR;

namespace FileFlow.Application.MessageBus;

internal class EventBus : IEventBus
{
    private readonly IMediator _mediator;

    public EventBus(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IEvent
    {
        return _mediator.Publish(@event, cancellationToken);
    }
}