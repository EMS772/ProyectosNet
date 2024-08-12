using Microsoft.EntityFrameworkCore;
using Tienda_NetCore.Models.Data;
using Tienda_NetCore.Models.Entidades;

namespace Tienda_NetCore.Services
{
    public class ProductoService:IProductoService
    {
        private readonly TiendaContext _context;

        public ProductoService(TiendaContext context)
        {
            _context = context;
        }

        public IEnumerable<Producto> ObtenerTodos()
        {
            return _context.Productos.ToList();
        }

        public Producto ObtenerporID(int id)
        {
            return _context.Productos.Include(p => p.Categoria).FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Producto> ObtenerPorCategoria(int categoriaId)
        {
            return _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.CategoriaId == categoriaId)
                .ToList();
        }
    }
}
