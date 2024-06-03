using FluentValidation;
using Hackney.Core.Validation;

namespace HousingSearchApi.V1.Boundary.Requests.Validation
{
    public class GetAllTenureListRequestValidator : AbstractValidator<GetAllTenureListRequest>
    {
        public GetAllTenureListRequestValidator()
        {
            When(x => !string.IsNullOrEmpty(x.LastHitTenureStartDate), () =>
            {
                RuleFor(x => x.LastHitTenureStartDate).NotXssString();

                RuleFor(x => x.LastHitTenureStartDate)
                .Custom((tenureStartDate, context) =>
                {
                    var dateIsInCorrectFormat = long.TryParse(tenureStartDate, out var dateInMilliseconds);

                    if (!dateIsInCorrectFormat)
                    {
                        context.AddFailure("LastHitTenureStartDate must be provided in milliseconds since epoch format");
                    }
                });
            });
        }
    }
}


