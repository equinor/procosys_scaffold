using FluentValidation;

namespace Equinor.ProCoSys.PCS5.WebApi.Controllers.Foo
{
    public class EditLinkDtoValidator : AbstractValidator<EditLinkDto>
    {
        public EditLinkDtoValidator(IRowVersionValidator rowVersionValidator)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto).NotNull();

            RuleFor(dto => dto.Title)
                .NotNull()
                .MaximumLength(Domain.AggregateModels.LinkAggregate.Link.TitleLengthMax);

            RuleFor(dto => dto.Url)
                .MaximumLength(Domain.AggregateModels.LinkAggregate.Link.UrlLengthMax);

            RuleFor(dto => dto.RowVersion)
                .NotNull()
                .Must(HaveValidRowVersion)
                .WithMessage(dto => $"Dto does not have valid rowVersion! RowVersion={dto.RowVersion}");

            bool HaveValidRowVersion(string rowVersion)
                => rowVersionValidator.IsValid(rowVersion);
        }
    }
}
