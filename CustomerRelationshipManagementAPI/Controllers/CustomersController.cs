using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CustomerRelationshipManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository customerRepository;
        private readonly ILogger<Customer> _logger;
        private readonly IMapper _mapper;

        public CustomersController(ICustomerRepository customerRepository, IMapper mapper, ILogger<Customer> logger)
        {
            this.customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<CustomerDto>> GetCustomersListAsync(
            string? phoneNumber,
            [FromQuery] string? searchQuery,
            int pageNumber=0,
            int pageSize = 10)
        {
            try
            {
                var (customers , paginationMetaData) 
                    = await customerRepository.GetAllAsync(phoneNumber, searchQuery, pageNumber, pageSize);
                var dtos = _mapper.Map<List<CustomerDto>>(customers);
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetaData));
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}" , Name = "GetCustomerAsyncByID")]
        public async Task<ActionResult<CustomerDto>> GetCustomerAsync(int id,bool includeRequests = false)
        {
            try
            {
                if (!await customerRepository.CheckCustomerIfExisted(id))
                {
                    _logger.LogWarning($"User with ID {id} not found");
                    return NotFound($"Customer with ID {id} not found");
                }
           
                Customer customer = await customerRepository.GetCustomerByIdAsync(id, includeRequests);
                if (includeRequests)
                {
                    CustomerWithRequestsDto cstdto = _mapper.Map<CustomerWithRequestsDto>(customer);
                    return Ok(cstdto);
                }

                CustomerDto dto = _mapper.Map<CustomerDto>(customer);
                return Ok(dto);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<Customer>> AddNewCustomer([FromBody] CustomerDto dto)
        {
            using (_logger.BeginScope("Adding New Customer {UserName} for phone {Phone}", dto.FullName, dto.PhoneNumber))
            {
                try
                {
                    if (await customerRepository.CheckPhoneNumberIfRegistered(dto.PhoneNumber))
                    {
                        _logger.LogWarning("Processing {@customer}", dto);
                        ModelState.AddModelError("PhoneNumber", "Phone Number is already existed before !");
                    }
                       
                    if (ModelState.IsValid)
                    {
                        Customer customer = _mapper.Map<Customer>(dto);
                        await customerRepository.AddNewCustomer(customer);
                        await customerRepository.SaveChangesAsync();

                        //return Ok(customer);
                        return CreatedAtRoute("GetCustomerAsyncByID", new { id = customer.Id }, dto);
                    }

                    return BadRequest(string.Join('&',
                        ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return BadRequest(ex.Message);
                }
            }
        }
        [HttpPut("{customerID}")]
        public async Task<ActionResult> UpdateCustomerFully(int customerID,[FromBody] CustomerDto customerDto)
        {
            try
            {
                if (!await customerRepository.CheckCustomerIfExisted(customerID))
                    return NotFound($"Customer with ID {customerID} Not Found");

                var customer = await customerRepository.GetCustomerByIdAsync(customerID, false);

                _mapper.Map(customerDto, customer);
                await customerRepository.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("{customerID}")]
        public async Task<ActionResult> UpdateCustomerPartially(int customerID, [FromBody] JsonPatchDocument<CustomerDto> customerPatchDto)
        {
            try
            {
                if (!await customerRepository.CheckCustomerIfExisted(customerID))
                    return NotFound($"Customer with ID {customerID} Not Found");

                var customer = await customerRepository.GetCustomerByIdAsync(customerID, false);

                var customerToPatch = _mapper.Map<CustomerDto>(customer);

                customerPatchDto.ApplyTo(customerToPatch, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!TryValidateModel(customerToPatch))
                    return BadRequest(ModelState);

                _mapper.Map(customerToPatch, customer);
                await customerRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{customerId}")]
        public async Task<ActionResult> DeleteCustomer(int customerId)
        {
            try
            {
                if (!await customerRepository.CheckCustomerIfExisted(customerId))
                    return NotFound($"Customer with ID {customerId} Not Found");

                var customer = await customerRepository.GetCustomerByIdAsync(customerId,false);
                customerRepository.DeleteCustomer(customer);
                await customerRepository.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
