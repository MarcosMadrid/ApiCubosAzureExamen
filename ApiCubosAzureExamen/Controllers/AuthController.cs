using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json.Serialization;
using ApiCubosAzureExamen.Helpers;
using ApiCubosAzureExamen.Models;
using ApiCubosAzureExamen.Repositories;

namespace ApiCubosAzureExamen.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private RepositoryCubosSqlServer repositoryCubos;
        private HelperActionServicesOauth helperOauth;

        public AuthController(RepositoryCubosSqlServer repositoryHospital, HelperActionServicesOauth servicesOauth)
        {
            this.repositoryCubos = repositoryHospital;
            this.helperOauth = servicesOauth;
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            Usuario? user = await repositoryCubos.LogIn(model.Email, model.Password);
            if (user == null)
            {
                return Unauthorized();
            }
            else
            {
                string jsonEmp = JsonConvert.SerializeObject(user);
                Claim[] claims = new[]
                {
                    new Claim("UserData", jsonEmp)
                };
                SigningCredentials credentials = new SigningCredentials(this.helperOauth.GetKeyToken(), SecurityAlgorithms.HmacSha256);
                JwtSecurityToken token = new JwtSecurityToken(
                    claims: claims,
                    issuer: helperOauth.Issuer,
                    audience: helperOauth.Audience,
                    signingCredentials: credentials,
                    expires: DateTime.UtcNow.AddMinutes(15),
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
