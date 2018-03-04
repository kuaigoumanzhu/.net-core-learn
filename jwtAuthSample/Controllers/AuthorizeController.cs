using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using jwtAuthSample.Models;
using jwtAuthSample.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace jwtAuthSample.Controllers
{
    [Route("api/[controller]")]
    public class AuthorizeController : Controller
    {
        private readonly JwtSettings _jwtSettings;

        public AuthorizeController(IOptions<JwtSettings> jwtSettingsAccesser)
        {
            _jwtSettings = jwtSettingsAccesser.Value;
        }
        public IActionResult Token(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (!(viewModel.User == "han" && viewModel.Password == "123456"))
                {
                    return BadRequest();
                }

                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name,"han"), 
                    new Claim(ClaimTypes.Role,"admin"),
                    new Claim("SuperAdminOnly","true")
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
                var creds=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claims,
                    DateTime.Now,DateTime.Now.AddMinutes(30), creds);
                return Ok(new {token = new JwtSecurityTokenHandler().WriteToken(token)});
            }
            return BadRequest();
        }
    }
}