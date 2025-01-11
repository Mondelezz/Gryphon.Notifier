using FluentValidation;

namespace Application.Features.EventFeatures.Command;

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
                RuleFor(request => request.EventCreateOrUpdateDto)
                    .NotNull()
                    .SetValidator(new ValidatorEventCreateOrUpdateDto());
            }
        }

        public class ValidatorEventCreateOrUpdateDto : AbstractValidator<EventCreateOrUpdateDto>
        {
            public ValidatorEventCreateOrUpdateDto()
            {
                RuleFor(o => o.Price)
                    .InclusiveBetween(0, decimal.MaxValue)
                    .WithMessage($"Значение должно быть в диапазоне от 0 до {decimal.MaxValue} .");

                RuleFor(o => o.Description)
                    .MaximumLength(300)
                    .WithMessage($"Максимальная длина описания должна быть 300 символов");

                RuleFor(date => date.DateEvent)
                    .Must(date => date > DateTime.UtcNow)
                    .WithMessage("Значение должно быть больше текущей даты.");
            }
        }
    }
}
