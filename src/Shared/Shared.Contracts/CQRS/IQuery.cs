using MediatR;

namespace Shared.Contracts.CQRS;

public interface IQuery<TResponse> : IRequest<TResponse>
{
}
