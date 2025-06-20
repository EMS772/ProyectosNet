using Microsoft.EntityFrameworkCore;
using MoneyMindIA.Models.Entidades;

namespace MoneyMindIA.Models.Data
{
    public class MoneyMindDbContext : DbContext
    {
        public MoneyMindDbContext(DbContextOptions<MoneyMindDbContext> options)
            : base(options)
        {
        }

        // DbSets (Tablas de la base de datos)
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Billetera> Billeteras { get; set; }
        public DbSet<Tarjeta> Tarjetas { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Meta> Metas { get; set; }
        public DbSet<ChatMensaje> ChatMensajes { get; set; }
        public DbSet<Recomendacion> Recomendaciones { get; set; }

        // Configuración del modelo de datos
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //--------------------- CONFIGURACIONES DE ENTIDADES ---------------------

            // Entidad Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.UsuarioId);
                entity.Property(u => u.Nombre).HasMaxLength(100).IsRequired();
                entity.Property(u => u.Email).IsRequired();
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.PasswordHash);
                entity.Property(u => u.EsRegistroNormal).IsRequired();
                entity.Property(u => u.FechaRegistro).HasDefaultValueSql("GETUTCDATE()");
            });

            // Entidad Billetera
            modelBuilder.Entity<Billetera>(entity =>
            {
                entity.HasKey(b => b.BilleteraId);
                entity.Property(b => b.PayPalEmail).IsRequired();
                entity.Property(b => b.PayPalAccessToken).IsRequired();
                entity.Property(b => b.BalanceActual)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);

                // Relación 1:1 con Usuario
                entity.HasOne(b => b.Usuario)
                    .WithOne(u => u.Billetera)
                    .HasForeignKey<Billetera>(b => b.UsuarioId)
                    .OnDelete(DeleteBehavior.NoAction);

                modelBuilder.Entity<Billetera>(entity =>
                {
                    entity.HasMany(b => b.Tarjetas)
                        .WithOne(t => t.Billetera)
                        .HasForeignKey(t => t.BilleteraId)
                        .OnDelete(DeleteBehavior.Cascade);
                });
            });

            // Entidad Tarjeta
            modelBuilder.Entity<Tarjeta>(entity =>
            {
                entity.HasKey(t => t.TarjetaId);
                entity.Property(t => t.NombreTitular).HasMaxLength(100).IsRequired();
                entity.Property(t => t.NumeroTarjeta).HasMaxLength(16).IsRequired();
                entity.Property(t => t.FechaExpiracion).HasMaxLength(5).IsRequired();
                entity.Property(t => t.CVV).HasMaxLength(3).IsRequired();
                entity.Property(t => t.Saldo)
                    .HasColumnType("decimal(18,2)")  // Configuración explícita para decimal
                    .HasDefaultValue(0);

                // Relación con Billetera
                entity.HasOne(t => t.Billetera)
                    .WithMany(b => b.Tarjetas)
                    .HasForeignKey(t => t.BilleteraId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Entidad Transaccion
            modelBuilder.Entity<Transaccion>(entity =>
            {
                entity.HasKey(t => t.TransaccionId);
                entity.Property(t => t.Monto)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                entity.Property(t => t.Fecha).IsRequired();
                entity.Property(t => t.Descripcion).HasMaxLength(200);
                entity.Property(t => t.Tipo).IsRequired();

                // Relación con Billetera
                entity.HasOne(t => t.Billetera)
                    .WithMany(b => b.Transacciones)
                    .HasForeignKey(t => t.BilleteraId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relación con Categoría
                entity.HasOne(t => t.Categoria)
                    .WithMany(c => c.Transacciones)
                    .HasForeignKey(t => t.CategoriaId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Relación con Tarjeta (Sin eliminación en cascada)
                entity.HasOne(t => t.Tarjeta)
                    .WithMany()
                    .HasForeignKey(t => t.TarjetaId)
                    .OnDelete(DeleteBehavior.NoAction);

                // Relación con Usuario
                entity.HasOne(t => t.Usuario)
                    .WithMany(u => u.Transacciones)
                    .HasForeignKey(t => t.UsuarioId)
                    .OnDelete(DeleteBehavior.NoAction); // O Restrict, según tu lógica
            });

            // Entidad Categoría
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(c => c.CategoriaId);
                entity.Property(c => c.Nombre)
                    .HasMaxLength(100)
                    .IsRequired();
                entity.HasIndex(c => c.Nombre).IsUnique();
            });

            // Entidad Meta
            modelBuilder.Entity<Meta>(entity =>
            {
                entity.HasKey(m => m.MetaId);
                entity.Property(m => m.Descripcion)
                    .HasMaxLength(200)
                    .IsRequired();
                entity.Property(m => m.MontoObjetivo)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                entity.Property(m => m.MontoActual)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);
                entity.Property(m => m.FechaCreacion)
                    .HasDefaultValueSql("GETUTCDATE()");
                entity.HasIndex(m => m.FechaCreacion); // Índice en FechaCreacion

                // Relación con Usuario
                entity.HasOne(m => m.Usuario)
                    .WithMany(u => u.Metas)
                    .HasForeignKey(m => m.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ChatMensaje>(entity =>
            {
                entity.HasKey(c => c.MensajeId);

                entity.Property(c => c.Contenido)
                    .IsRequired()
                    .HasMaxLength(4000); // Aumenta el límite si es necesario

                entity.Property(c => c.EsUsuario)
                    .IsRequired()
                    .HasDefaultValue(false); // Valor por defecto

                entity.Property(c => c.FechaEnvio)
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()"); // Fecha automática

                entity.HasOne(c => c.Recomendacion)
                    .WithMany(r => r.Mensajes)
                    .HasForeignKey(c => c.RecomendacionId)
                    .OnDelete(DeleteBehavior.ClientCascade); // Cambio clave aquí
            });

            // Configuración para Recomendacion
            modelBuilder.Entity<Recomendacion>(entity =>
            {
                entity.HasKey(r => r.RecomendacionId);
                entity.Property(r => r.Mensaje)
                    .HasMaxLength(1500)
                    .IsRequired();

                // Relación con Usuario
                entity.HasOne(r => r.Usuario)
                    .WithMany(u => u.Recomendaciones)
                    .HasForeignKey(r => r.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}