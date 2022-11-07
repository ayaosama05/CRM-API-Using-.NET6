using CustomerRelationshipManagementAPI.Core.Helpers;

namespace CustomerRelationshipManagementAPI.Core.Interfaces
{
    public interface IRequestRepository
    {
        Task<(IEnumerable<Request>, PaginationMetaData)> GetAllRequestsAsync(int? requestType, int pageNumber, int pageSize);
        Task AddNewRequest(int cstID,Request request);
        Task<bool> SaveChangesAsync();

        Task<bool> CheckRequestIfExisted(int requestID);
        Task<Request?> GetRequestForCustomerAsync(int customerID, int requestID);
        Task<Request> GetRequestByIDAsync(int requestID);

        void DeleteRequest(Request request);
    }
}
