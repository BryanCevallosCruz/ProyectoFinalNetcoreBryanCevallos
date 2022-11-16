
using System.Diagnostics;
using Curso.ComercioElectronico.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Curso.ComercioElectronico.Infraestructure;

public static class InfraestructureServiceCollectionExtensions
{

    public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration config)
    {

        services.AddTransient<IMarcaRepository, MarcaRepository>();
        services.AddTransient<IProductoRepository, ProductoRepository>(); 
        services.AddTransient<ITipoProductoRepository, TipoProductoRepository>(); 
        services.AddTransient<IClienteRepository, ClienteRepository>();
        services.AddTransient<IBancoClienteRepository, BancoClienteRepository>();
        services.AddTransient<ICarroRepository, CarroRepository>(); 
        services.AddTransient<ICarroItemRepository, CarroItemRepository>(); 
        services.AddTransient<IOrdenRepository, OrdenRepository>(); 
        services.AddTransient<IOrdenItemRepository, OrdenItemRepository>(); 

        //Configuraciones de Dependencias
        //Configurar DBContext
        services.AddDbContext<ComercioElectronicoDbContext>(options =>
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            var dbPath = Path.Join(path, config.GetConnectionString("ComercioElectronico"));
            Debug.WriteLine($"dbPath: {dbPath}");
            Console.WriteLine($"dbPath: {dbPath}");

            //Utilizar la base de datos sqlite
            options.UseSqlite($"Data Source={dbPath}");
        });

        //Utilizar una factoria
        services.AddScoped<IUnitOfWork>(provider =>
        {
            var instance = provider.GetService<ComercioElectronicoDbContext>();
            return instance;
        });


            return services;

    }
}