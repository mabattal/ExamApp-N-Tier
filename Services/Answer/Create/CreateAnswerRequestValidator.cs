using FluentValidation;

namespace ExamApp.Services.Answer.Create
{
    public class CreateAnswerRequestValidator : AbstractValidator<CreateAnswerRequestDto>
    {
        public CreateAnswerRequestValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId required and must be greater than 0.");

            RuleFor(x => x.ExamId)
                .GreaterThan(0).WithMessage("ExamId required and must be greater than 0.");

            RuleFor(x => x.QuestionId)
                .GreaterThan(0).WithMessage("QuestionId required and must be greater than 0.");
        }
    }
}
