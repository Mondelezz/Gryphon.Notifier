using Microsoft.AspNetCore.Mvc;
using Application.Features.EventFeatures.Command;
using Mediator;

namespace API.Controllers;

/// <summary>
/// Отвечает за работу с событиями
/// </summary>
[Route("api/v1/event")]
[ApiController]
public class EventController(IMediator mediator) : ControllerBase
{
    [HttpPost("create-event")]
    public async Task<ActionResult<long>> CreateEvent(
        EventCreate.RequestDto requestDto,
        string currentUserId,
        CancellationToken cancellationToken = default) => await mediator.Send(new EventCreate.Command(requestDto, currentUserId), cancellationToken);
}
