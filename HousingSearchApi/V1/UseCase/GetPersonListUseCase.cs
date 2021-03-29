using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using HousingSearchApi.V1.Domain;
using HousingSearchApi.V1.Gateways;
using HousingSearchApi.V1.UseCase.Interfaces;

namespace HousingSearchApi.V1.UseCase
{
    public class GetPersonListUseCase : IGetPersonListUseCase
    {
        private readonly ISearchPersonsGateway _searchPersonsGateway;
        private readonly IGetPersonListRequestValidator _validator;

        public GetPersonListUseCase(ISearchPersonsGateway searchPersonsGateway, IGetPersonListRequestValidator validator)
        {
            _searchPersonsGateway = searchPersonsGateway;
            _validator = validator;
        }

        public async Task<GetPersonListResponse> ExecuteAsync(GetPersonListRequest getPersonListRequest)
        {
            return await _searchPersonsGateway.GetListOfPersons(getPersonListRequest);
        }
    }


    public interface IGetPersonListRequestValidator
    {
        ValidationResult Validate(GetPersonListRequest getPersonListRequest);
    }

    public class GetPersonListRequestValidator : IGetPersonListRequestValidator
    {
        public ValidationResult Validate(GetPersonListRequest getPersonListRequest)
        {
            throw new NotImplementedException();
        }
    }
}
