using Microsoft.AspNetCore.Mvc;
using Mediator;
using Application.Features.EventFeatures.Query.EventGet;
using Application.Features.EventFeatures.Query.EventListGet;
using Application.Features.EventFeatures.Command.EventCreateOrUpdate;
using System.ComponentModel.DataAnnotations;
using Application.Features.EventFeatures.Command.AddEventToGroupEvent;

namespace API.Controllers;

/// <summary>
/// Отвечает за работу с событиями
/// </summary>
/// <param name="mediator">IMediator</param>
[Route("api/v1/event")]
[ApiController]
public class EventController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Создание или редактирование события
    /// </summary>
    /// <param name="requestDto">Событие</param>
    /// <param name="eventId">Идентификатор события</param>
    /// <param name="groupEventId">Идентификатор группы, в которой создаётся событие</param>
    /// <param name="currentUserId">Идентификатор текущего пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор созданной или обновлённой сущности</returns>
    [HttpPost("create-or-update-event")]
    public async Task<ActionResult<long>> CreateOrUpdateEventAsync(
        EventCreateOrUpdate.RequestDto requestDto,
        long? eventId,
        long? groupEventId,
        string currentUserId,
        CancellationToken cancellationToken = default) => await mediator.Send(
            new EventCreateOrUpdate.Command(requestDto, eventId, groupEventId, currentUserId), cancellationToken);

    /// <summary>
    /// Добавляет событие к указанной группе
    /// </summary>
    /// <param name="currentUserId">Идентификатор текущего пользователя</param>
    /// <param name="groupEventId">Идентификатор группы, куда следует переместить событие</param>
    /// <param name="eventId">Идентификатор события</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор добавленного события</returns>
    [HttpPost("add-to-group")]
    public async Task<ActionResult<long>> AddEventToGroupEvent(
        string currentUserId,
        long groupEventId,
        long eventId,
        CancellationToken cancellationToken = default) => await mediator.Send(
            new AddEventToGroupEvent.Command(
                currentUserId,
                groupEventId,
                eventId), cancellationToken);

    /// <summary>
    /// Получение события по идентификатору
    /// </summary>
    /// <param name="currentUserId">Идентификатор текущего пользователя</param>
    /// <param name="eventId">Идентификатор события</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Событие</returns>
    [HttpGet("{eventId}")]
    public async Task<EventGet.ResponseDto> GetEventAsync(
        string currentUserId,
        long eventId,
        CancellationToken cancellationToken = default) => await mediator.Send(
            new EventGet.Query(currentUserId, eventId), cancellationToken);

    /// <summary>
    /// Получение списка событий с возможностью филтрации, сортировки и пагинации
    /// </summary>
    /// <param name="currentUserId">Идентификатор текущего пользователя</param>
    /// <param name="offset">Количество элементов на странице</param>
    /// <param name="skipCount">Количество элементов для пропуска</param>
    /// <param name="sorting">Критерий сортировки.</param>
    /// <param name="sortByDescending">Флаг для сортировки по убыванию.</param>
    /// <param name="requestFilter">Фильтр для поиска организаций.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Список событий</returns>
    [HttpGet]
    public async Task<EventListGet.ResponseDto> GetEventListAsync(
        string currentUserId,
        [FromQuery][Range(1, 100)] int offset = 10,
        [FromQuery][Range(0, int.MaxValue)] int skipCount = 10,
        [FromQuery] EventListGet.Sorting sorting = EventListGet.Sorting.DateEvent,
        [FromQuery] bool sortByDescending = false,
        [FromQuery] EventListGet.RequestFilter? requestFilter = default,
        CancellationToken cancellationToken = default) => await mediator.Send(
            new EventListGet.Query(
                currentUserId,
                offset, skipCount,
                sorting,
                sortByDescending,
                requestFilter), cancellationToken);
}
