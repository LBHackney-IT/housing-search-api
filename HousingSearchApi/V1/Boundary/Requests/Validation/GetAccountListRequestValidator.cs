using FluentValidation;
using Hackney.Core.Validation;

namespace HousingSearchApi.V1.Boundary.Requests.Validation
{
    public class GetAccountListRequestValidator : AbstractValidator<GetAccountListRequest>
    {
        public GetAccountListRequestValidator()
        {
            RuleFor(x => x.SearchText).NotNull()
                .DependentRules(() =>
                {
                    RuleFor(x => x.TargetId).Null();
                    RuleFor(x => x.TargetId).Empty();
                })
                .NotEmpty()
                .DependentRules(() =>
                {
                    RuleFor(x => x.TargetId).Null();
                    RuleFor(x => x.TargetId).Empty();
                })
                .MaximumLength(2)
                .NotXssString();

            RuleFor(x => x.TargetId).NotNull()
                .DependentRules(() =>
                {
                    RuleFor(x => x.SearchText).Null();
                    RuleFor(x => x.SearchText).Empty();
                })
                .NotEmpty()
                .DependentRules(() =>
                {
                    RuleFor(x => x.SearchText).Null();
                    RuleFor(x => x.SearchText).Empty();
                })
                .NotXssString();

            RuleFor(x => x.PageSize).GreaterThan(0);
            RuleFor(x => x.SortBy).NotXssString();
        }
    }
}
