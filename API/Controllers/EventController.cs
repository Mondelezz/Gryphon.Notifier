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
    /// <summary>
    /// Создание или редактирование события
    /// </summary>
    /// <param name="requestDto">Событие</param>
    /// <param name="eventId">Идентификатор события</param>
    /// <param name="currentUserId">Идентификатор текущего пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор созданной сущности</returns>
    [HttpPost("create-or-update-event")]
    public async Task<ActionResult<long>> CreateOrUpdateEvent(
        EventCreateOrUpdate.RequestDto requestDto,
        long? eventId,
        string currentUserId,
        CancellationToken cancellationToken = default) => await mediator.Send(
            new EventCreateOrUpdate.Command(requestDto, eventId, currentUserId), cancellationToken);

}
