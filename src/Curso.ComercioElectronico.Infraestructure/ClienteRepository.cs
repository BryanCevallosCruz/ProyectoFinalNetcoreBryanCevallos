using Curso.ComercioElectronico.Domain;
using Microsoft.EntityFrameworkCore;

namespace Curso.ComercioElectronico.Infraestructure;

public class ClienteRepository : EfRepository<Cliente, Guid>, IClienteRepository
{
        // se asume que la clase abstracta EfRepository ya implementa 6 metodos
    public ClienteRepository(ComercioElectronicoDbContext context) : base(context)
    {
    }

    public async Task<bool> ExisteNombre(string nombre)
    {
        var resultado = await this._context.Set<Cliente>()
                        .AnyAsync(x => x.Nombre.ToUpper() == nombre.ToUpper());

        return resultado;
    }

    public async Task<bool> ExisteNombre(string nombre, Guid idExcluir)
    {
        var query =  this._context.Set<Cliente>()
                       .Where(x => x.Id != idExcluir)
                       .Where(x => x.Nombre.ToUpper() == nombre.ToUpper())
                       ;

        var resultado = await query.AnyAsync();

        return resultado;
    }

    public async Task<ICollection<Cliente>> GetListAsync(IList<Guid> listaIds, bool asNoTracking = true)
    {
        var consulta = GetAll(asNoTracking);

        consulta = consulta.Where(
                x => listaIds.Contains(x.Id)
            );

        //select * from productos where id in (1,3,4,5)
        return await consulta.ToListAsync();
    }
}