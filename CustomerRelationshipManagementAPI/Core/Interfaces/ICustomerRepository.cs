

using CustomerRelationshipManagementAPI.Core.Helpers;

namespace CustomerRelationshipManagementAPI.Core.Interfaces
{
    public interface ICustomerRepository
    {
        Task<(IEnumerable<Customer>, PaginationMetaData)> GetAllAsync(string? phoneNumber, string? searchQuery, int pageNumber, int pageSize);
        Task<Customer> GetCustomerByIdAsync(int customerId,bool includedRequestsHistory);
        Task<bool> CheckCustomerIfExisted(int customerId);
        Task AddNewCustomer(Customer customer);
        Task<bool> SaveChangesAsync();
        void DeleteCustomer(Customer customer);
        Task<bool> CheckPhoneNumberIfRegistered(string phoneNumber);
    }
}
