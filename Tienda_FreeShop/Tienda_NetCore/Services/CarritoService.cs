using System;
using Tienda_NetCore.Models.Data;

namespace Tienda_NetCore.Services
{
    public class CarritoService:ICarritoService
    {
        private readonly TiendaContext _context;

        public CarritoService(TiendaContext context)
        {
            _context = context;
        }

        public void EliminarDelCarrito(int id)
        {
            // Encuentra el ítem del carrito por su Id
            var item = _context.CarritoItems.Find(id);
            if (item != null)
            {
                // Elimina el ítem del carrito
                _context.CarritoItems.Remove(item);
                _context.SaveChanges();
            }
        }
    }
}
