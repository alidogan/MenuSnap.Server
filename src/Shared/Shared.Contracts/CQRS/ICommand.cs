using MediatR;

namespace Shared.Contracts.CQRS;

public interface ICommand : IRequest<Unit>
{
}

public interface ICommand<TResponse> : IRequest<TResponse>
{
}
