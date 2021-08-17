using Nest;

namespace HousingSearchApi.V1.Interfaces
{
    public interface IIndexSelector
    {
        Indices.ManyIndices Create<T>();
    }
}
