using Mediator;

namespace Application.Common;

public interface ICommandRequest<out TResponce> : IRequest<TResponce>
{
}

public interface ICommandRequest : IRequest
{
}
