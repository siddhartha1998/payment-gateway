using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PaymentGateway.Server.Application.Common.Interface;
using PaymentGateway.Server.Infrastructure.Identity;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PaymentGateway.Server.Api.Controllers
{
    [Authorize]
    public class AccountController : ApiControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IApplicationDbContext _dbContext;

        public AccountController(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration,
            IApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpPost("/api/login")]
        public async Task<ActionResult<string>> Authenticate([FromBody] AuthenticationRequest authenticationRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var identityUser = await _userManager.FindByEmailAsync(authenticationRequest.UserName);
            identityUser ??= await _userManager.FindByNameAsync(authenticationRequest.UserName);
          
            if (identityUser == null || identityUser.IsActive == false)
            {
                return Unauthorized();
            }
            List<Claim> userClaims = await GetClaimsAsync(identityUser);
            JwtSecurityToken token = GenerateJwtToken(userClaims);

            var tokenResult = new
            {
                User = new UserResponse
                {
                    UserName = identityUser.UserName,
                    Email = identityUser.Email,
                    PhoneNo = identityUser.PhoneNumber,
                    IsActive = identityUser.IsActive,
                    Roles = (await _userManager.GetRolesAsync(identityUser)).ToList(),
                },
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };

            return Ok(tokenResult);

        }

        private async Task<List<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            List<Claim> roleClaims = roles.Select(role => new Claim("roles",role)).ToList();
            List<Claim> userClaims = (await _userManager.GetClaimsAsync(user)).ToList();
            userClaims = userClaims.Union(roleClaims).ToList();
            userClaims = new List<Claim>(userClaims)
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new(JwtRegisteredClaimNames.Email, user.Email),
            };

            return userClaims;
        }

        private JwtSecurityToken GenerateJwtToken(List<Claim> userClaims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ValidMinutes"])),
                signingCredentials: creds
                );

            return token;
        }
    }
    public class AuthenticationRequest
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
    }

    public class UserResponse
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNo { get; set; }
        public bool IsActive { get; set; }
        public List<string> Roles { get; set; }
    }

}
