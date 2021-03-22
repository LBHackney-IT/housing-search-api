using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using SearchApi.V1.Domain;
using SearchApi.V1.Gateways;
using IPersonsGateway = SearchApi.V1.UseCase.IPersonsGateway;

namespace SearchApi.Tests.V1.UseCase
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
