using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.DataAccess.Contexts;
using WebApplication1.Infrastructure.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        public AuthController(IConfiguration configuration, AppDbContext dbContext, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet("google-login")]
        public async Task LoginGoogle()
        {
            await HttpContext.ChallengeAsync("Google", new AuthenticationProperties() { RedirectUri = "/" });
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Create([FromBody]RegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = _mapper.Map<IdentityUser>(model);

            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded && result.Errors.Any())
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }

                return BadRequest(ModelState);
            }

            return CreatedAtRoute("GetById",
                new { id = userIdentity.Id },
                userIdentity);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]RegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = await GetClaimsIdentity(model.UserName, model.Password);
            if (identity == null)
            {
                ModelState.AddModelError("login_failure", "Invalid username or password.");
                return BadRequest(ModelState);
            }

            var jwt = GetJwtToken(identity);
            return Ok(jwt);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            var userToVerify = await _userManager.FindByNameAsync(userName);

            if (userToVerify == null)
            {
                userToVerify = await _userManager.FindByEmailAsync(userName);
                if (userToVerify == null)
                {
                    return await Task.FromResult<ClaimsIdentity>(null);
                }
            }
            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                var claims = await _userManager.GetClaimsAsync(userToVerify);
                return await Task.FromResult(new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme));
            }
            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        private string GetJwtToken(ClaimsIdentity identity)
        {
            var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                notBefore: DateTime.UtcNow,
                claims: identity.Claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(1)),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}
