using ApiCubosAzureExamen.Data;
using ApiCubosAzureExamen.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCubosAzureExamen.Repositories
{
    public class RepositoryCubosSqlServer
    {
        private CubosContext cubosContext;

        public RepositoryCubosSqlServer(CubosContext cubosContext)
        {
            this.cubosContext = cubosContext;
        }

        public async Task<List<Cubo>?> GetCubos()
        {
            return
                await cubosContext.Cubos.ToListAsync();
        }

        public async Task<List<Cubo>?> GetCubosMarca(string marca)
        {
            return
                await cubosContext.Cubos
                .Where(C => C.Marca!.Equals(marca))
                .ToListAsync();
        }

        public async Task<int> GetMaxIdUser()
        {
            return
                await cubosContext.Usuarios
                .Select(user => user.Id)
                .MaxAsync();
        }

        public async Task<Usuario> PostUser(string nombre, string email, string pass, string imagen)
        {
            Usuario usuario = new Usuario()
            {
                Nombre = nombre,
                Email = email,
                Password = pass,
                Imagen = imagen,
                Id = await GetMaxIdUser() + 1
            };

            await cubosContext.Usuarios.AddAsync(usuario);
            await cubosContext.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario?> LogIn(string email, string pass)
        {
            return
                await cubosContext.Usuarios
                .FirstOrDefaultAsync(user => user.Email!.Equals(email) && user.Password!.Equals(pass));
        }
    }
}
