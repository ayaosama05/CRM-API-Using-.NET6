using CustomerRelationshipManagementAPI.Core.Models;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CustomerRelationshipManagementAPI.Core.Repositories
{
    public class AuthRepository: IAuthRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager; 
        private readonly IConfiguration _configuration;
        public AuthRepository(UserManager<IdentityUser> userManager,
             RoleManager<IdentityRole> roleManager,
             IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<JwtSecurityToken> GetTokenAsync(IdentityUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in userRoles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authClaims = new List<Claim> {
                  new Claim(ClaimTypes.Name,user.UserName),
                  new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                  new Claim(JwtRegisteredClaimNames.Email,user.Email),
                  new Claim("uid",user.Id),
            }.Union(userClaims).Union(roleClaims);

            var authSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Authentication:SecretForKey"]));

            var signingCredentials = new SigningCredentials(authSecurityKey,SecurityAlgorithms.HmacSha256);

            var expirationDuration = Convert.ToDouble(_configuration["Authentication:DurationInHours"]);

            var JWTSecurityToken = new JwtSecurityToken(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                expires: DateTime.Now.AddHours(expirationDuration),
                claims: authClaims,
                signingCredentials: signingCredentials);

            return JWTSecurityToken;
        }

        public async Task<AuthModel> LoginAsync(LoginModel model)
        {
            var authModel = new AuthModel();
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user is null || !await _userManager.CheckPasswordAsync(user,model.Password))
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "Username or password is incorrect !";
                return authModel;
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim> {
                  new Claim(ClaimTypes.Name,user.UserName),
                  new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                  new Claim(JwtRegisteredClaimNames.Email,user.Email),
                  new Claim("uid",user.Id),
            };
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = await GetTokenAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            authModel.Roles = userRoles.ToList();
            authModel.TokenExpirationDate = token.ValidTo;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(token);

            return authModel;
        }

        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null)
                return new AuthModel { IsAuthenticated = false, Message = "Email is already registered" };

            if (await _userManager.FindByNameAsync(model.UserName) != null)
                return new AuthModel { IsAuthenticated = false, Message = "UserName is already registered" };

            var user = new IdentityUser
            {
                UserName = model.UserName,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if(!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var error in result.Errors)
                    errors += $"{ error.Description } ,";

                return new AuthModel { IsAuthenticated = false, Message = errors };
            }


            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            await _userManager.AddToRoleAsync(user, UserRoles.User);
        //    await _userManager.AddToRoleAsync(user, UserRoles.Admin);

            var jwtSewcurityToken = await GetTokenAsync(user);

            return new AuthModel
            {
                Email = user.Email,
                TokenExpirationDate = jwtSewcurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSewcurityToken),
                Username = user.UserName
            };
        }
    }
}
