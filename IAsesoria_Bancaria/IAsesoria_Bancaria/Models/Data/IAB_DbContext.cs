using IAsesoria_Bancaria.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace IAsesoria_Bancaria.Models.Data
{
    public class IAB_DbContext: DbContext
    {
        public IAB_DbContext(DbContextOptions<IAB_DbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Usuario
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();

                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();

                entity.HasMany(e => e.Conversations)
                      .WithOne(e => e.User)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Conversación
            modelBuilder.Entity<Conversation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();

                entity.HasIndex(e => e.UserId);

                entity.HasMany(e => e.Messages)
                      .WithOne(e => e.Conversation)
                      .HasForeignKey(e => e.ConversationId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Mensaje
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Role).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.Timestamp).IsRequired();

                entity.HasIndex(e => e.ConversationId);
            });
        }
    }
}
