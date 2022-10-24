using FluentValidation;
using Hackney.Core.Validation;
using System;

namespace HousingSearchApi.V1.Boundary.Requests.Validation
{
    public class GetProcessListRequestValidator : AbstractValidator<GetProcessListRequest>
    {
        public GetProcessListRequestValidator()
        {
            RuleFor(x => x.SearchText).NotNull().When(x => x.TargetId == null & x.TargetType == null);
            RuleFor(x => x.PageSize).GreaterThan(0);
            RuleFor(x => x.SortBy).NotXssString();
            RuleFor(x => x.TargetId).NotNull().When(x => x.TargetType != null);
            RuleFor(x => x.TargetType).NotNull().When(x => x.TargetId != null);

        }
    }
}
