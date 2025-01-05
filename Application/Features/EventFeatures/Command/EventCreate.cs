using Application.Common;
using Mediator;

namespace Application.Features.EventFeatures.Command;

public static partial class EventCreate
{
    public record Command(
        RequestDto RequestDto,
        long CurrentUserId) : ICommandRequest<long>;

    public class Handler() : IRequestHandler<Command, long>
    {
        public ValueTask<long> Handle(Command request, CancellationToken cancellationToken)
        {

        }

        private async Task CreateEventAsync()
        {

        }
    }
}
