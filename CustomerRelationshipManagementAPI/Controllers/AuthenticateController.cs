using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace CustomerRelationshipManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly ILogger<IdentityUser> _logger;

        public AuthenticateController(IAuthRepository authRepository,
            ILogger<IdentityUser> logger,
            IConfiguration configuration)
        {
            _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var authResultModel = await _authRepository.LoginAsync(model);
                if(!authResultModel.IsAuthenticated)
                    return Forbid(authResultModel.Message);

                return Ok(authResultModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var authResultModel = await _authRepository.RegisterAsync(model);
                if (!authResultModel.IsAuthenticated)
                    return BadRequest(authResultModel.Message);

                return Ok(authResultModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
