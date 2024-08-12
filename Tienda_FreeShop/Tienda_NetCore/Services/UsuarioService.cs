using Microsoft.EntityFrameworkCore;
using Tienda_NetCore.Models.Data;
using Tienda_NetCore.Models.Entidades;

namespace Tienda_NetCore.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly TiendaContext _context;

        public UsuarioService(TiendaContext context)
        {
            _context = context;
        }

        public async Task<Usuario> ObtenerUsuarioPorEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
