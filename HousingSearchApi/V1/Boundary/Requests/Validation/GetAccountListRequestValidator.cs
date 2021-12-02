using System;
using FluentValidation;
using Hackney.Core.Validation;

namespace HousingSearchApi.V1.Boundary.Requests.Validation
{
    public class GetAccountListRequestValidator : AbstractValidator<GetAccountListRequest>
    {
        public GetAccountListRequestValidator()
        {
            RuleFor(x => x.SearchText).NotEmpty()
                .When(w => w.TargetId == Guid.Empty)
                .NotNull()
                .When(w => w.TargetId == Guid.Empty)
                .MinimumLength(2).When(x => !string.IsNullOrEmpty(x.SearchText))
                .NotXssString();

            RuleFor(x => x.TargetId).NotEmpty()
                .When(w => string.IsNullOrEmpty(w.SearchText));

            RuleFor(x => x.PageSize).GreaterThan(0);
            RuleFor(x => x.SortBy).NotXssString();

            RuleFor(x => x.Page).GreaterThan(-1);
        }
    }
}
