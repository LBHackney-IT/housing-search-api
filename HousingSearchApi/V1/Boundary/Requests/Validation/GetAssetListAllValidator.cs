using FluentValidation;
using Hackney.Core.Validation;

namespace HousingSearchApi.V1.Boundary.Requests.Validation
{
    public class GetAssetListAllValidator : AbstractValidator<GetAllAssetListRequest>
    {
        public GetAssetListAllValidator()
        {
            RuleFor(x => x.SearchText).MinimumLength(2)
                                      .NotXssString();
            RuleFor(x => x.PageSize).GreaterThan(0);
            RuleFor(x => x.SortBy).NotXssString();
        }
    }
}
