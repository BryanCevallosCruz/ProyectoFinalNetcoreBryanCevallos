using Curso.ComercioElectronico.Domain;
using Microsoft.EntityFrameworkCore;

namespace Curso.ComercioElectronico.Infraestructure;

public class CarroRepository : EfRepository<Carro, Guid>, ICarroRepository
{
        // se asume que la clase abstracta EfRepository ya implementa 6 metodos
    public CarroRepository(ComercioElectronicoDbContext context) : base(context)
    {
    }


    public async Task<ICollection<Carro>> GetListAsync(IList<Guid> listaIds, bool asNoTracking = true)
    {
        var consulta = GetAll(asNoTracking);

        consulta = consulta.Where(
                x => listaIds.Contains(x.Id)
            );

        //select * from productos where id in (1,3,4,5)
        return await consulta.ToListAsync();
    }
}

public class CarroItemRepository : EfRepository<CarroItem, Guid>, ICarroItemRepository
{
        // se asume que la clase abstracta EfRepository ya implementa 6 metodos
    public CarroItemRepository(ComercioElectronicoDbContext context) : base(context)
    {
    }


    public async Task<ICollection<CarroItem>> GetListAsync(IList<Guid> listaIds, bool asNoTracking = true)
    {
        var consulta = GetAll(asNoTracking);

        consulta = consulta.Where(
                x => listaIds.Contains(x.Id)
            );

        //select * from productos where id in (1,3,4,5)
        return await consulta.ToListAsync();
    }
}