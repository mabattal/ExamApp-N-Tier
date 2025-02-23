using FluentValidation;

namespace ExamApp.Services.Answer
{
    public class CreateAnswerRequestValidator : AbstractValidator<CreateAnswerRequestDto>
    {
        public CreateAnswerRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId required.");

            RuleFor(x => x.ExamId)
                .NotEmpty().WithMessage("ExamId required.");

            RuleFor(x => x.QuestionId)
                .NotEmpty().WithMessage("QuestionId required.");
        }
    }
}
