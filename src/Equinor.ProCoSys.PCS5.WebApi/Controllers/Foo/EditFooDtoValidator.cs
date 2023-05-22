using FluentValidation;

namespace Equinor.ProCoSys.PCS5.WebApi.Controllers.Foo
{
    public class EditFooDtoValidator : AbstractValidator<EditFooDto>
    {
        public EditFooDtoValidator(IRowVersionValidator rowVersionValidator)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto).NotNull();

            RuleFor(dto => dto.Title)
                .NotNull()
                .MinimumLength(Domain.AggregateModels.FooAggregate.Foo.TitleLengthMin)
                .MaximumLength(Domain.AggregateModels.FooAggregate.Foo.TitleLengthMax);

            RuleFor(dto => dto.Text)
                .MaximumLength(Domain.AggregateModels.FooAggregate.Foo.TextLengthMax);

            RuleFor(dto => dto.RowVersion)
                .NotNull()
                .Must(HaveValidRowVersion)
                .WithMessage(dto => $"Dto does not have valid rowVersion! RowVersion={dto.RowVersion}");

            bool HaveValidRowVersion(string rowVersion)
                => rowVersionValidator.IsValid(rowVersion);
        }
    }
}
