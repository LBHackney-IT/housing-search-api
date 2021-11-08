using HousingSearchApi.V1.Boundary.Requests;
using HousingSearchApi.V1.Boundary.Responses.Transactions;
using System.Threading.Tasks;

namespace HousingSearchApi.V1.UseCase.Interfaces
{
    public interface IGetTransactionListUseCase
    {
        Task<GetTransactionListResponse> ExecuteAsync(TransactionSearchRequest transactionSearchRequest);
    }
}
