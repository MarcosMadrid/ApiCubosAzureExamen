using ApiCubosAzureExamen.Models;
using ApiCubosAzureExamen.Repositories;
using ApiCubosAzureExamen.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ApiCubosAzureExamen.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CubosController : ControllerBase
    {
        private RepositoryCubosSqlServer repositoryCubos;
        private ServiceAzureStorage azureStorage;

        public CubosController(RepositoryCubosSqlServer cubosSqlServer, ServiceAzureStorage azureStorage)
        {
            repositoryCubos = cubosSqlServer;
            this.azureStorage = azureStorage;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cubo>?>> GetCubos()
        {
            try
            {
                List<Cubo>? cubos = await repositoryCubos.GetCubos();
                foreach (Cubo cubo in cubos)
                {
                    cubo.Imagen = azureStorage.GetContainerUrl("imagenes") + "/" + cubo.Imagen;
                }
                return cubos;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<Usuario?>> PerfilUser()
        {
            try
            {
                Claim? claim = User.FindFirst(claim => claim.Type == "UserData");
                if (claim == null)
                {
                    return NoContent();
                }
                else
                {
                    Usuario user = JsonConvert.DeserializeObject<Usuario>(claim.Value)!;
                    user.Imagen = azureStorage.GetContainerUrl("imagenes") + "/" + user.Imagen;
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
