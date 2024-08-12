using Tienda_NetCore.Models.Entidades;

namespace Tienda_NetCore.Services
{
    public interface IUsuarioService
    {
        Task<Usuario> ObtenerUsuarioPorEmail(string email);

    }
}
