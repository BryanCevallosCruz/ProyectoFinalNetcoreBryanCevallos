using Curso.ComercioElectronico.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace Curso.ComercioElectronico.Infraestructure;


public class ComercioElectronicoDbContext : DbContext, IUnitOfWork
{

    //Agregar sus entidades
    public DbSet<Marca> Marcas { get; set; }

    public DbSet<TipoProducto> TipoProductos { get; set; }
    
    public DbSet<Producto> Productos { get; set; }

    public DbSet<Cliente> Clientes { get; set; }
 
    public DbSet<BancoCliente> BancoClientes { get; set; }

    public DbSet<Carro> Carros { get; set; }
    public DbSet<CarroItem> CarroItems { get; set; }
    public DbSet<Orden> Ordenes { get; set; }
    public DbSet<OrdenItem> OrdenItems { get; set; }


    public string DbPath { get; set; }

    public ComercioElectronicoDbContext(DbContextOptions<ComercioElectronicoDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
         //#Ref: https://learn.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
          modelBuilder.Entity<Producto>()
            .Property(e => e.Precio)
            .HasConversion<double>()
            ;

           /*
            modelBuilder.Entity<OrdenItem>()
            .Property(e => e.Id)
            .HasConversion<string>();
          */

          modelBuilder.Entity<CarroItem>()
            .Property(e => e.Precio)
            .HasConversion<double>();
            modelBuilder.Entity<OrdenItem>()
            .Property(e => e.Precio)
            .HasConversion<double>();

    }

}



