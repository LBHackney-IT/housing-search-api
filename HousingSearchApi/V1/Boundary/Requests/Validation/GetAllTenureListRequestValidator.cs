using FluentValidation;
using Hackney.Core.Validation;
using System;

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

                RuleFor(x => x.LastHitId).NotEmpty().WithMessage("LastHitId must be provided with LastHitTenureStartDate");
            });

            RuleFor(x => x.LastHitId).NotXssString();

            When(x => !string.IsNullOrEmpty(x.LastHitId), () =>
                RuleFor(x => x.LastHitId)
                    .Custom((lastHitId, context) =>
                    {
                        var lastHitIdGuid = Guid.TryParse(lastHitId, out var guid);

                        if (!lastHitIdGuid)
                        {
                            context.AddFailure("LastHitId is not in a valid Guid format");
                        };

                    })
             );
        }
    }
}


