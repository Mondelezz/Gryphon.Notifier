using Mediator;

namespace Application.Common;

public interface IQueryRequest<out TResponce> : IRequest<TResponce>
{
}

