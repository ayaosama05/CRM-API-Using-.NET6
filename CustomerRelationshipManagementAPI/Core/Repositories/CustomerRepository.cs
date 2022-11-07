using CustomerRelationshipManagementAPI.Core.Helpers;
using CustomerRelationshipManagementAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomerRelationshipManagementAPI.Core.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<(IEnumerable<Customer>, PaginationMetaData)> GetAllAsync(string? phoneNumber,
            string? searchQuery, int pageNumber, int pageSize)
        {
            var customers = _context.Customers as IQueryable<Customer>;

            if(!string.IsNullOrEmpty(phoneNumber))
                customers = customers.Where(x => x.PhoneNumber == phoneNumber.Trim());

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim().ToLower();
                customers = customers.Where(cst =>
                cst.FirstName.ToLower().Contains(searchQuery)
                || (cst.PhoneNumber.Contains(searchQuery))
                || (cst.Email.ToLower().Contains(searchQuery))
                || (cst.LastName != null && cst.LastName.ToLower().Contains(searchQuery))
                );
            }

            var totalCount = await customers.CountAsync();

            var paginationMetaData = new PaginationMetaData(pageNumber, pageSize, totalCount);
            var customersToReturn = await customers
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (customersToReturn, paginationMetaData);
        }
        public async Task AddNewCustomer(Customer customer)
        {
           await _context.AddAsync(customer);
        }
        public async Task<bool> CheckCustomerIfExisted(int customerId) => await _context.Customers.AnyAsync(a => a.Id == customerId);
        public async Task<bool> CheckPhoneNumberIfRegistered(string phoneNumber) => await _context.Customers.AnyAsync(a => a.PhoneNumber == phoneNumber);

        public void DeleteCustomer(Customer customer) => _context.Customers.Remove(customer);

        public async Task<Customer> GetCustomerByIdAsync(int customerId, bool includedRequestsHistory)
        {
            if(includedRequestsHistory)
                return await _context.Customers
                    .Include( a => a.Requests)
                    .ThenInclude( a => a.RequestType)
                    .FirstAsync(a => a.Id == customerId);

            return await _context.Customers.FirstAsync(a=> a.Id == customerId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}
