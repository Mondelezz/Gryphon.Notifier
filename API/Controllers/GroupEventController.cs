using Mediator;
using Microsoft.AspNetCore.Mvc;
using Application.Features.EventFeatures.Command.GroupEventCreateOrUpdate;

namespace API.Controllers;

/// <summary>
/// Отвечает за управление группами для событий
/// </summary>
/// <param name="mediator"></param>
[Route("api/v1/group-events")]
[ApiController]
public class GroupEventController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Создание или редактирование группы для событий
    /// </summary>
    /// <param name="requestDto">Группа для событий</param>
    /// <param name="currentUserId">Идентификатор текущего пользователя</param>
    /// <param name="groupEventId">Идентификатор группы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Идентификатор созданной или обновлённой сущности</returns>
    [HttpPost("create-or-update-group")]
    public async Task<ActionResult<long>> CreateOrUpdateGroupAsync(
        GroupEventCreateOrUpdate.RequestDto requestDto,
        string currentUserId,
        long? groupEventId,
        CancellationToken cancellationToken = default) => await mediator.Send
        (new GroupEventCreateOrUpdate.Command(
            currentUserId,
            groupEventId,
            requestDto), cancellationToken);
}
