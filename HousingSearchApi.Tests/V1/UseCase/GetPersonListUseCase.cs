using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using HousingSearchApi.V1.Domain;
using IPersonsGateway = HousingSearchApi.V1.UseCase.IPersonsGateway;

namespace HousingSearchApi.Tests.V1.UseCase
{
    public class GetPersonListUseCase : IGetPersonListUseCase
    {
        private readonly IPersonsGateway _personsGateway;
        private readonly IGetPersonListRequestValidator _validator;

        public GetPersonListUseCase(IPersonsGateway personsGateway, IGetPersonListRequestValidator validator)
        {
            _personsGateway = personsGateway;
            _validator = validator;
        }

        public Task<GetPersonListResponse> ExecuteAsync(GetPersonListRequest getPersonListRequest)
        {
            throw new NotImplementedException();
        }
    }



    public interface IGetPersonListRequestValidator
    {
        ValidationResult Validate(GetPersonListRequest getPersonListRequest);
    }
}
