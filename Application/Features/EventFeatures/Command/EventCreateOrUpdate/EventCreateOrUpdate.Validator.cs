using FluentValidation;

namespace Application.Features.EventFeatures.Command.EventCreateOrUpdate;

public static partial class EventCreateOrUpdate
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(u => u.RequestDto)
                .NotNull()
                .SetValidator(new ValidatorRequestDto());
        }

        public class ValidatorRequestDto : AbstractValidator<RequestDto>
        {
            public ValidatorRequestDto()
            {
                RuleFor(request => request.EventDto)
                    .NotNull()
                    .SetValidator(new ValidatorEventDto());
            }
        }

        public class ValidatorEventDto : AbstractValidator<EventDto>
        {
            public ValidatorEventDto()
            {
                RuleFor(o => o.Price)
                    .InclusiveBetween(0, decimal.MaxValue)
                    .WithMessage($"Значение должно быть в диапазоне от 0 до {decimal.MaxValue} .");

                RuleFor(o => o.Name)
                    .MinimumLength(1)
                        .WithMessage("Минимальная длина названия события 1 символ")
                    .MaximumLength(60)
                        .WithMessage("Максимальная длина названия события 60 символов");

                RuleFor(o => o.Description)
                    .MinimumLength(1)
                        .WithMessage("Минимальная длина описания 1 символ")
                    .MaximumLength(300)
                        .WithMessage("Максимальная длина описания 300 символов");

                /*RuleFor(date => date.DateEvent)
                    .Must(date => date > DateTime.UtcNow)
                    .WithMessage("Событие не может начаться в прошлом.");*/ //Хочу реализовать возможность создания событий, которые уже в прошлом (для памяти, что оно прошло)

                RuleFor(date => date.TimeEventStart)
                    .Must((eventModel, startTime) => startTime < eventModel.TimeEventEnded)
                    .WithMessage("Время начала события не может быть позже времени окончания.");

                RuleFor(date => date.TimeEventEnded)
                    .Must((eventModel, endTime) => endTime > eventModel.TimeEventStart)
                    .WithMessage("Время окончания события не может быть раньше времени начала.");
            }
        }
    }
}
