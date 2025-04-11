using Microsoft.AspNetCore.Mvc;
using Application.Features.TopicFeatures.Command;
using Application.Features.TopicFeatures.Query;
using Application.Interfaces;

namespace API.Controllers;

/// <summary>
/// Отвечает за управление топиками
/// </summary>
/// <param name="topicService">topicService</param>
[Route("api/v1/topic")]
[ApiController]
public class TopicController(ITopicService topicService) : ControllerBase
{
    /// <summary>
    /// Создание или редактирование топика
    /// </summary>
    /// <param name="requestDto">Топик</param>
    /// <param name="currentUserId">Идентификатор текущего пользователя</param>
    /// <param name="topicId">Идентификатор топика</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор созданной или обновлённой сущности</returns>
    [HttpPost("create-or-update")]
    public async Task<ActionResult<long>> CreateOrUpdateTopicAsync(
        CreateOrUpdateTopic.RequestDto requestDto,
        long currentUserId,
        long? topicId,
        CancellationToken cancellationToken = default) =>
        await topicService.CreateOrUpdateTopicAsync(currentUserId, topicId, requestDto, cancellationToken);

    /// <summary>
    /// Получение списка топиков
    /// </summary>
    /// <param name="currentUserId">Идентификатор текущего пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список топиков</returns>
    [HttpGet]
    public async Task<ActionResult<GetListTopic.ResponseDto>> GetListTopicAsync(
        long currentUserId,
        CancellationToken cancellationToken = default) =>
        await topicService.GetListTopicAsync(currentUserId, cancellationToken);

    /// <summary>
    /// Получение топика и входящих в него событий
    /// </summary>
    /// <param name="currentUserId">Идентификатор текущего пользователя</param>
    /// <param name="topicId">Идентификатор топика</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список топиков</returns>
    [HttpGet("{topicId}")]
    public async Task<ActionResult<GetTopic.ResponseDto>> GetTopicAsync(
        long currentUserId,
        long topicId,
        CancellationToken cancellationToken = default) =>
        await topicService.GetTopicByIdAsync(currentUserId, topicId, cancellationToken);
}
