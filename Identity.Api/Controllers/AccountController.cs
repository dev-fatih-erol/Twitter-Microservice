using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Identity.Api.Data.Entities;
using Identity.Api.Extensions;
using Identity.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Api.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Account/Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if (result.Succeeded)
                {
                    var date = DateTime.UtcNow;
                    var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat, date.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
                        };
                    var securityKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_configuration["JwtConfiguration:SecurityKey"]));
                    var securityToken = new JwtSecurityToken(
                        issuer: _configuration["JwtConfiguration:Issuer"],
                        audience: _configuration["JwtConfiguration:Audience"],
                        claims: claims,
                        notBefore: date,
                        expires: date.AddMinutes(60),
                        signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                    );

                    var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
                    return Ok(token);
                }
            }

            return BadRequest("Please try another email address or password.");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Account/Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            var user = new User
            {
                Name = request.Name,
                Surname = request.Surname,
                UserName = request.UserName,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                return Created($"User/{user.Id}", null);
            }

            return BadRequest(result.GetError());
        }
    }
}