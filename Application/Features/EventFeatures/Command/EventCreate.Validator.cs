using FluentValidation;

namespace Application.Features.EventFeatures.Command;

public static partial class EventCreate
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

                RuleFor(date => date.DateEvent)
                .Must(date => date > DateTime.UtcNow)
                .WithMessage("Значение должно быть больше текущей даты.");
            }
        }
    }
}
