using CustomerRelationshipManagementAPI.Core.Helpers;
using CustomerRelationshipManagementAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomerRelationshipManagementAPI.Core.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ICustomerRepository _customerRepository;

        public RequestRepository(ApplicationDbContext context,
            ICustomerRepository customerRepository)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        public async Task AddNewRequest(int cstID, Request request)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(cstID,false);
            if (customer != null)
                customer.Requests.Add(request);
        }

        public async Task<(IEnumerable<Request> , PaginationMetaData)> GetAllRequestsAsync(int? requestType, int pageNumber, int pageSize)
        {
            var requests = _context.Requests as IQueryable<Request>;

            if (requestType != null)
                requests = requests.Where(a => a.RequestTypeId == requestType);

            int totalCount = await requests.CountAsync();

            PaginationMetaData paginationMetaData = new PaginationMetaData(pageNumber,pageSize,totalCount);

            var requestsToReturn = await requests
                .Include(a => a.Customer)
                .Include(b => b.RequestType)
                .Skip((pageNumber-1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (requestsToReturn, paginationMetaData);
        }

        public async Task<bool> CheckRequestIfExisted(int requestID)
        {
            return await _context.Requests.AnyAsync(a => a.Id == requestID);
        }

        public async Task<Request> GetRequestByIDAsync(int requestID)
        {
            return await _context.Requests
                .Include(a => a.Customer)
                .Include(a => a.RequestType)
                .FirstAsync(a => a.Id == requestID);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false ;
        }

        public void DeleteRequest(Request request)
        {
            _context.Requests.Remove(request);
        }

        public async Task<Request?> GetRequestForCustomerAsync(int customerID, int requestID) => await _context.Requests
                .FirstOrDefaultAsync(a => a.CustomerId == customerID && a.Id == requestID);
    }
}
