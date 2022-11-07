using AutoMapper;
using CustomerRelationshipManagement.API.Services;
using CustomerRelationshipManagementAPI.Core.Models;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CustomerRelationshipManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestRepository _requestRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<Request> _logger;
        private readonly IMapper _mapper;
        private readonly ITwilioService _twillioService;

        public RequestsController(
            IRequestRepository requestRepository,
            ICustomerRepository customerRepository,
            ILogger<Request> logger,
            IMapper mapper,
            ITwilioService twillioService)
        {
            _requestRepository = requestRepository ?? throw new ArgumentNullException(nameof(requestRepository));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _twillioService = twillioService ?? throw new ArgumentNullException(nameof(twillioService));
        }

        [HttpGet]
        public async Task<ActionResult> GetAllRequests(int? requestType,int pageNumber =0,int pageSize=10)
        {
            try
            {
                var (requests , PaginationmetaData) = 
                    await _requestRepository.GetAllRequestsAsync(requestType, pageNumber, pageSize);
                var requestsDtos = _mapper.Map<IEnumerable<RequestDto>>(requests);

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(PaginationmetaData));

                return Ok(requestsDtos);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{requestID}" , Name = "GetRequestByID")]
        public async Task<ActionResult<RequestDto>> GetRequest(int requestID)
        {
            try
            {
                if (!await _requestRepository.CheckRequestIfExisted(requestID))
                    return NotFound($"There's no request found with ID {requestID}");

                var request = await _requestRepository.GetRequestByIDAsync(requestID);
                if(request == null)
                    return NotFound("Request not found");

                var requestDto = _mapper.Map<RequestDto>(request);
                return Ok(requestDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{customerID}")]
        public async Task<ActionResult> AddCustomerRequest(int customerID,[FromBody]RequestForCreationDto requestDto)
        {
            try
            {
                if (!await _customerRepository.CheckCustomerIfExisted(customerID))
                    return NotFound($"There's no customer with ID {customerID}");

                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                var request = _mapper.Map<Request>(requestDto);

                await _requestRepository.AddNewRequest(customerID,request);
                await _requestRepository.SaveChangesAsync();

                var requestToMap = await _requestRepository.GetRequestByIDAsync(request.Id);
                var requestDtoToReturn = 
                    _mapper.Map<RequestDto>(requestToMap);

                var phoneNumber = requestDtoToReturn.CustomerPhone.StartsWith("+20") ?
                    requestDtoToReturn.CustomerPhone : 
                    (requestDtoToReturn.CustomerPhone.StartsWith("0") ?
                    $"+2{requestDtoToReturn.CustomerPhone}" : $"+20{requestDtoToReturn.CustomerPhone}");

                var message = new Message(
                    $"{phoneNumber}",
                    $"تم تسجيل طلبك برقم {request.Id} و سيكون الرد فى خلال 48 ساعه من خدمه العملاء");

                var result = _twillioService.Send(message);

                if (!result.IsValid)
                    return Ok($"Request created with No. {request.Id} but an error has been occured while" +
                        $" sending whatsapp message to your phone number");
                else
                    return Ok($"Whatsapp message is sent to your phone number with details");

            //   return CreatedAtRoute(
            //        routeName: "GetRequestByID",
            //        routeValues: new { requestID = requestDtoToReturn.Id },
            //        value: requestDtoToReturn);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException?.Message);
                return BadRequest(ex.InnerException?.Message);
            }
        }

        [HttpPut("{customerID}")]
        public async Task<ActionResult> UpdateRequest(int customerID , [FromBody] RequestForUpdateDto dto)
        {
            try
            {
                if (!await _customerRepository.CheckCustomerIfExisted(customerID))
                    return NotFound($"There's no customer with ID {customerID}");

                var request = await _requestRepository.GetRequestForCustomerAsync(customerID, dto.Id);
                if (request == null)
                    return NotFound("Request Not Found");

                _mapper.Map(dto , request);
                await _requestRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException?.Message);
                return BadRequest(ex.InnerException?.Message);
            }
        }

        [HttpDelete("{requestID}")]
        public async Task<ActionResult> DeleteRequest(int requestID)
        {
            try
            {
                if (!await _requestRepository.CheckRequestIfExisted(requestID))
                    return NotFound($"There's no request with ID {requestID}");

                var requestToDelete = await _requestRepository.GetRequestByIDAsync(requestID);
                _requestRepository.DeleteRequest(requestToDelete);
                await _requestRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException?.Message);
                return BadRequest(ex.InnerException?.Message);
            }
        }
    }
}
