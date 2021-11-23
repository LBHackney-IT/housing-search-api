using HousingSearchApi.V1.Boundary.Requests;

namespace HousingSearchApi.V1.Interfaces.Factories
{
    public interface IQueryFactory
    {
        IQueryGenerator<T> CreateQuery<T>(HousingSearchRequest request) where T : class;
    }
}
