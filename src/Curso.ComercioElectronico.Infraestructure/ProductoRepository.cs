
using Curso.ComercioElectronico.Domain;
using Microsoft.EntityFrameworkCore;

namespace Curso.ComercioElectronico.Infraestructure;

public class ProductoRepository : EfRepository<Producto, Guid>, IProductoRepository
{
        // se asume que la clase abstracta EfRepository ya implementa 6 metodos
    public ProductoRepository(ComercioElectronicoDbContext context) : base(context)
    {
    }

    public async Task<bool> ExisteNombre(string nombre) {

        var resultado = await this._context.Set<Producto>()
                       .AnyAsync(x => x.Nombre.ToUpper() == nombre.ToUpper());

        return resultado;
    }

    public async Task<bool> ExisteNombre(string nombre, Guid idExcluir)  {

        var query =  this._context.Set<Producto>()
                       .Where(x => x.Id != idExcluir)
                       .Where(x => x.Nombre.ToUpper() == nombre.ToUpper())
                       ;

        var resultado = await query.AnyAsync();

        return resultado;
    }

    public async Task<ICollection<Producto>> GetListAsync(IList<Guid> listaIds, bool asNoTracking = true)
    {
        //GetAll, se ejecuta el linq???
        var consulta = GetAll(asNoTracking);

        consulta = consulta.Where(
                x => listaIds.Contains(x.Id)
            );

        //select * from productos where id in (1,3,4,5)
        return await consulta.ToListAsync();
       
    }
    // public async Task<List<Producto>> ListaProductosAsync (List<int> idProducto){
    //     var consulta = new List<Producto>();
    //     foreach (var id in idProducto)
    //     {
    //         var producto = await this.GetByIdAsync(id);
    //         if(producto != null ){
    //             consulta.Add(producto);
    //         }
    //     }
    //     return consulta;
    // }
}