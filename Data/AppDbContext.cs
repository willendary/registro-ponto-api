using Microsoft.EntityFrameworkCore;
using RegistroDoPonto.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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

        // Configura a conversão de DateTime para UTC
        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.ToUniversalTime(), // Converte para UTC antes de salvar
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc) // Especifica o Kind como UTC ao ler
        );

        modelBuilder.Entity<RegistroDoPonto.Models.RegistroDoPonto>()
            .Property(r => r.DataHora)
            .HasConversion(dateTimeConverter);
    }
}