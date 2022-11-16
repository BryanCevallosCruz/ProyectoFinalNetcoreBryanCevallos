using Curso.ComercioElectronico.Domain;
using Microsoft.EntityFrameworkCore;

namespace Curso.ComercioElectronico.Infraestructure;

public class OrdenRepository : EfRepository<Orden, Guid>, IOrdenRepository
{
        // se asume que la clase abstracta EfRepository ya implementa 6 metodos
    public OrdenRepository(ComercioElectronicoDbContext context) : base(context)
    {
    }


    public async Task<ICollection<Orden>> GetListAsync(IList<Guid> listaIds, bool asNoTracking = true)
    {
        var consulta = GetAll(asNoTracking);

        consulta = consulta.Where(
                x => listaIds.Contains(x.Id)
            );

        //select * from productos where id in (1,3,4,5)
        return await consulta.ToListAsync();
    }
}

public class OrdenItemRepository : EfRepository<OrdenItem, Guid>, IOrdenItemRepository
{
        // se asume que la clase abstracta EfRepository ya implementa 6 metodos
    public OrdenItemRepository(ComercioElectronicoDbContext context) : base(context)
    {
    }


    public async Task<ICollection<OrdenItem>> GetListAsync(IList<Guid> listaIds, bool asNoTracking = true)
    {
        var consulta = GetAll(asNoTracking);

        consulta = consulta.Where(
                x => listaIds.Contains(x.Id)
            );

        //select * from productos where id in (1,3,4,5)
        return await consulta.ToListAsync();
    }
}