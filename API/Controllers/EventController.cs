using Microsoft.AspNetCore.Mvc;
using Application.Features.EventFeatures.Query;
using Application.Features.EventFeatures.Command;
using System.ComponentModel.DataAnnotations;
using Application.Interfaces;
using Application.Features.TopicFeatures.Query;

namespace API.Controllers;

/// <summary>
/// Отвечает за работу с событиями
/// </summary>
/// <param name="eventService">eventService</param>
[Route("api/v1/event")]
[ApiController]
public class EventController(IEventService eventService) : ControllerBase
{
    /// <summary>
    /// Создание или редактирование события
    /// </summary>
    /// <param name="eventDto">Событие</param>
    /// <param name="eventId">Идентификатор события</param>
    /// <param name="topicId">Идентификатор топика, в котором создаётся событие</param>
    /// <param name="currentUserId">Идентификатор текущего пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор созданной или обновлённой сущности</returns>
    [HttpPost("create-or-update-event")]
    public async Task<ActionResult<long>> CreateOrUpdateEventAsync(
        CreateOrUpdateEvent.EventDto eventDto,
        long? eventId,
        long? topicId,
        long currentUserId,
        CancellationToken cancellationToken = default) => await eventService.CreateOrUpdateEventAsync(
            eventDto,
            eventId,
            topicId,
            currentUserId,
            cancellationToken);

    /// <summary>
    /// Добавляет событие в топик
    /// </summary>
    /// <param name="currentUserId">Идентификатор текущего пользователя</param>
    /// <param name="topicId">Идентификатор топика, куда следует переместить событие</param>
    /// <param name="eventId">Идентификатор события</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Топик</returns>
    [HttpPost("add-event-to-topic")]
    public async Task<ActionResult<GetTopic.ResponseDto>> AddEventToTopic(
        long currentUserId,
        long topicId,
        long eventId,
        CancellationToken cancellationToken = default) => await eventService.AddEventToTopicAsync(
            currentUserId,
            topicId,
            eventId,
            cancellationToken);

    /// <summary>
    /// Получение события по идентификатору
    /// </summary>
    /// <param name="currentUserId">Идентификатор текущего пользователя</param>
    /// <param name="eventId">Идентификатор события</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Событие</returns>
    [HttpGet("{eventId}")]
    public async Task<GetEvent.ResponseDto> GetEventAsync(
        long currentUserId,
        long eventId,
        CancellationToken cancellationToken = default) => await eventService.GetEventAsync(
            currentUserId,
            eventId,
            cancellationToken);

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
    public async Task<GetListEvent.ResponseDto> GetListEventAsync(
        long currentUserId,
        [FromQuery][Range(1, 100)] int offset = 10,
        [FromQuery][Range(0, int.MaxValue)] int skipCount = 0,
        [FromQuery] GetListEvent.Sorting sorting = GetListEvent.Sorting.DateEvent,
        [FromQuery] bool sortByDescending = false,
        [FromQuery] GetListEvent.RequestFilter? requestFilter = default,
        CancellationToken cancellationToken = default) => await eventService.GetListEventAsync(
            currentUserId,
            offset,
            skipCount,
            sorting,
            sortByDescending,
            requestFilter,
            cancellationToken);
}
