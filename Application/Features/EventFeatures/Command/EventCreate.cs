using Application.Common;
using Application.Repository.Abstraction.Command;
using Mediator;

namespace Application.Features.EventFeatures.Command;

public static partial class EventCreate
{
    public record Command(
        RequestDto RequestDto,
        long CurrentUserId) : ICommandRequest<long>;

    public class Handler(ICommandEventRepository commandEventRepository) : IRequestHandler<Command, long>
    {
        public ValueTask<long> Handle(Command request, CancellationToken cancellationToken)
        {

        }

        private async Task CreateEventAsync()
        {

        }
    }
}
