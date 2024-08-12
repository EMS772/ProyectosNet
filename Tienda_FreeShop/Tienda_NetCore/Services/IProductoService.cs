using Tienda_NetCore.Models.Entidades;

namespace Tienda_NetCore.Services
{
    public interface IProductoService
    {
        IEnumerable<Producto> ObtenerTodos();
        Producto ObtenerporID(int id);
        IEnumerable<Producto> ObtenerPorCategoria(int categoriaId);

    }
}
