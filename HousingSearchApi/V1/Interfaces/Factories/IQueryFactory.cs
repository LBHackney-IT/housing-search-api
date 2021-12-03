namespace HousingSearchApi.V1.Interfaces.Factories
{
    public interface IQueryFactory
    {
        IQueryGenerator<T> CreateQuery<T>() where T : class;
    }
}
