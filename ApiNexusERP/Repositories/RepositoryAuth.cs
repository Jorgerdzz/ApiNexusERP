using ApiNexusERP.DTOs;
using ApiNexusERP.Helpers;
using Microsoft.EntityFrameworkCore;
using NugetModelsNexusERP.Data;
using NugetModelsNexusERP.Models;

namespace ApiNexusERP.Repositories
{
    public class RepositoryAuth
    {
        private NexusContext context;

        public RepositoryAuth(NexusContext context)
        {
            this.context = context;
        }

        public async Task<Usuario> LogInUserAsync(string email, string password)
        {
            var datosLogin = await (from u in this.context.Usuarios.IgnoreQueryFilters()
                                    join s in this.context.SeguridadUsuarios.IgnoreQueryFilters() on u.Id equals s.IdUsuario
                                    where u.Email == email
                                    select new { Usuario = u, Seguridad = s })
                                    .FirstOrDefaultAsync();

            if (datosLogin == null) return null;

            string saltBD = datosLogin.Seguridad.Salt;
            byte[] passwordHashBD = datosLogin.Seguridad.PasswordHash;

            byte[] passwordGenerado = HelperCryptography.EncryptPassword(password, saltBD);
            bool esValido = HelperTools.CompareArrays(passwordGenerado, passwordHashBD);

            return datosLogin.Usuario;           
        }
    }
}
