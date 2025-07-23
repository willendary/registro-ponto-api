using Microsoft.EntityFrameworkCore;
using RegistroDoPonto.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RegistroDoPonto.Data;

public class AppDbContext : IdentityDbContext<Usuario>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<RegistroDoPonto.Models.RegistroDoPonto> Registros { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RegistroDoPonto.Models.RegistroDoPonto>()
            .HasOne(r => r.Usuario)
            .WithMany(u => u.Registros)
            .HasForeignKey(r => r.UsuarioId);

        modelBuilder.Entity<RegistroDoPonto.Models.RegistroDoPonto>()
            .Property(r => r.Tipo)
            .HasConversion<string>();
    }
}