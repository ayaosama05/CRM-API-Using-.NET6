using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace CustomerRelationshipManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Must be an administrator")]
    public class AdministrationController : ControllerBase
    {
        private readonly IAdministrationRepository _administrationRepository;
        private readonly ILogger<IdentityRole> _logger;
        public AdministrationController(IAdministrationRepository administrationRepository,
              ILogger<IdentityRole> logger)
        {
            _administrationRepository = administrationRepository ?? 
                throw new ArgumentNullException(nameof(administrationRepository));
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("AddRole")]
        public async Task<ActionResult> AddRole(string roleName)
        {
            try
            {
                var result = await _administrationRepository.AddNewRoleAsync(roleName);
                if (!result.IsSucceeded)
                    return BadRequest(result.Message);

                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddUserRole")]
        public async Task<ActionResult> AddUserToRole(string userName , string roleName)
        {
            try
            {
                var result = await _administrationRepository.AddUserToRoleAsync(userName,roleName);
                if (!result.IsSucceeded)
                    return BadRequest(result.Message);

                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
