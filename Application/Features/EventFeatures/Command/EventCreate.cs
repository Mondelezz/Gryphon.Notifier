using Application.Common;
using Infrastructure.DbContexts;
using Mediator;

namespace Application.Features.EventFeatures.Command;

public static partial class EventCreate
{
    public record Command(
        RequestDto RequestDto,
        string CurrentUserId) : ICommandRequest<long>;

    public class Handler(CommandDbContext commandDbContext) : IRequestHandler<Command, long>
    {
        public ValueTask<long> Handle(Command request, CancellationToken cancellationToken)
        {
            
        }

        private async Task CreateEventAsync()
        {

        }
    }
}
