using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tienda_NetCore.Models.Entidades;

namespace Tienda_NetCore.Models.Data
{
    public class TiendaContext : IdentityDbContext<Usuario>
    {
        public TiendaContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<CarritoItem> CarritoItems { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoItem> PedidoItems { get; set; }
        public DbSet<VentaTemporal> VentasTemporales { get; set; }
        public DbSet<Venta> Ventas { get; set; } 


    }
}
