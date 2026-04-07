using ApiNexusERP.DTOs;
using ApiNexusERP.Helpers;
using ApiNexusERP.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NugetModelsNexusERP.Models;
using System.IdentityModel.Tokens.Jwt;

namespace ApiNexusERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private RepositoryAuth repo;
        private HelperActionOAuthService helper;

        public AuthController(RepositoryAuth repo, HelperActionOAuthService helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Login(LoginDTO model)
        {
            Usuario usuario = await this.repo.LogInUserAsync(model.Email, model.Password);
            if (usuario == null)
            {
                return Unauthorized(new {mensaje = "Email o contraseña incorrectos."});
            }
            else
            {
                SigningCredentials credentials =
                    new SigningCredentials(this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256);

                JwtSecurityToken token =
                    new JwtSecurityToken(
                        issuer: this.helper.Issuer,
                        audience: this.helper.Audience,
                        signingCredentials: credentials,
                        expires: DateTime.UtcNow.AddMinutes(20),
                        notBefore: DateTime.UtcNow
                        );

                return Ok(new
                {
                    response = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
        }

    }
}
