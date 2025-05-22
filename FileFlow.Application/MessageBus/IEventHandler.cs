using MediatR;

namespace FileFlow.Application.MessageBus;

internal interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : IEvent;