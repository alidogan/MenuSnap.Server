using MediatR;

namespace Shared.Contracts.CQRS;

public interface ICommandHandler<TCommand>
    : IRequestHandler<TCommand, Unit>
    where TCommand : ICommand
{
}

public interface ICommandHandler<TCommand, TResponse>
    : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
}
