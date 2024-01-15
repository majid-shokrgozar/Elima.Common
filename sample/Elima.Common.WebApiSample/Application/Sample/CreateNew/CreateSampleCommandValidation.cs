using FluentValidation;

namespace Elima.Common.WebApiSample.Application.Sample.CreateNew
{
    public class CreateSampleCommandValidation : AbstractValidator<CreateSampleCommand>
    {
        public CreateSampleCommandValidation()
        {
            RuleFor(sample => sample.Title)
                .NotEmpty().WithMessage("عنوان نباید خالی باشه")
                .MinimumLength(3).WithMessage("باید بیشتر از 3 حرف باشد").WithErrorCode("100");
                ;
        }
    }
}
