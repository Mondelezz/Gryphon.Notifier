using Mediator;
using Microsoft.AspNetCore.Mvc;
using Application.Features.EventFeatures.Command;
using Application.Features.EventFeatures.Query;

namespace API.Controllers;

/// <summary>
/// Отвечает за управление топиками
/// </summary>
/// <param name="mediator">mediator</param>
[Route("api/v1/topic")]
[ApiController]
public class TopicController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Создание или редактирование топика
    /// </summary>
    /// <param name="requestDto">Топик</param>
    /// <param name="currentUserId">Идентификатор текущего пользователя</param>
    /// <param name="topicId">Идентификатор топика</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор созданной или обновлённой сущности</returns>
    [HttpPost("create-or-update-topic")]
    public async Task<ActionResult<long>> CreateOrUpdateTopicAsync(
        CreateOrUpdateTopic.RequestDto requestDto,
        string currentUserId,
        long? topicId,
        CancellationToken cancellationToken = default) => await mediator.Send
            (new CreateOrUpdateTopic.Command(
                currentUserId,
                topicId,
                requestDto), cancellationToken);

    /// <summary>
    /// Получение списка топиков
    /// </summary>
    /// <param name="currentUserId">Идентификатор текущего пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список топиков</returns>
    [HttpGet]
    public async Task<ActionResult<TopicListGet.ResponseDto>> GetListTopicAsync(
        string currentUserId,
        CancellationToken cancellationToken = default) => await mediator.Send
            (new TopicListGet.Query(currentUserId), cancellationToken);
}
