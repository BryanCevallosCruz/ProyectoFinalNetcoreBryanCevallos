using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Curso.ComercioElectronico.Application;


public static class ApplicationServiceCollectionExtensions
{

    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
    {
        
        services.AddTransient<IBancoClienteAppService, BancoClienteAppService>();
        services.AddTransient<IClienteAppService, ClienteAppService>();
        services.AddTransient<IMarcaAppService, MarcaAppService>();
        services.AddTransient<ITipoProductoAppService, TipoProductoAppService>();
        services.AddTransient<IProductoAppService, ProductoAppService>();

        services.AddTransient<ICarroAppService, CarroAppService>();
        
        services.AddTransient<IOrdenAppService, OrdenAppService>();

        //Configurar la inyección de todos los profile que existen en un Assembly
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

       
        return services;

    }
}