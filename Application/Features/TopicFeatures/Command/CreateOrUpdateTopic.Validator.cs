using FluentValidation;

namespace Application.Features.TopicFeatures.Command;

public static partial class CreateOrUpdateTopic
{
    public class Validator : AbstractValidator<RequestDto>
    {
        public class ValidatorRequestDto : AbstractValidator<RequestDto>
        {
            public ValidatorRequestDto()
            {
                RuleFor(request => request.TopicDto)
                    .NotNull()
                    .SetValidator(new ValidatorTopicDto());
            }
        }

        public class ValidatorTopicDto : AbstractValidator<TopicDto>
        {
            public ValidatorTopicDto()
            {
                RuleFor(o => o.Name)
                    .MinimumLength(1)
                        .WithMessage("Минимальная длина названия топика 1 символ")
                    .MaximumLength(30)
                        .WithMessage("Максимальная длина названия топика 30 символов");
            }
        }
    }
}
