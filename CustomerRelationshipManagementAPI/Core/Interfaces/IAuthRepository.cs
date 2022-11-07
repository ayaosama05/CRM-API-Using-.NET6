using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CustomerRelationshipManagementAPI.Core.Interfaces
{
    public interface IAuthRepository
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> LoginAsync(LoginModel model);
        Task<JwtSecurityToken> GetTokenAsync(IdentityUser user);
    }
}
