using FluentValidation;

namespace Application.Features.EventFeatures.Command.GroupEventCreateOrUpdate;

public static partial class GroupEventCreateOrUpdate
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
                RuleFor(request => request.GroupEventDto)
                    .NotNull()
                    .SetValidator(new ValidatorGroupEventDto());
            }
        }

        public class ValidatorGroupEventDto : AbstractValidator<GroupEventDto>
        {
            public ValidatorGroupEventDto()
            {
                RuleFor(o => o.Name)
                    .MinimumLength(1)
                        .WithMessage($"Минимальная длина названия группы 1 символ")
                    .MaximumLength(30)
                        .WithMessage($"Максимальная длина названия группы 30 символов");
            }
        }
    }
}
