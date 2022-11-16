using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Curso.ComercioElectronico.Domain;

namespace Curso.ComercioElectronico.Application;

public class TipoProductoAppService : ITipoProductoAppService
{
    private readonly ITipoProductoRepository repository;
    private readonly IMapper mapper;
    

    public TipoProductoAppService(ITipoProductoRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<TipoProductoDto> CreateAsync(TipoProductoCrearDto tipoDto)
    {
        
        //Reglas Validaciones... 
        var existeNombreTipo = await repository.ExisteNombre(tipoDto.Nombre);
        if (existeNombreTipo){
            throw new ArgumentException($"Ya existe un tipo de pruducto con el nombre {tipoDto.Nombre}");
        }
 
        //Mapeo Dto => Entidad
        var tipo = mapper.Map<TipoProducto>(tipoDto);
 
        //Persistencia objeto
        tipo = await repository.AddAsync(tipo);
        await repository.UnitOfWork.SaveChangesAsync();

        //Mapeo Entidad => Dto
        var tipoCreado = mapper.Map<TipoProductoDto>(tipo);


        return tipoCreado;
    }

    public async Task UpdateAsync(string id, TipoProductoActualizarDto tipoDto)
    {
        var tipo = await repository.GetByIdAsync(id);
        if (tipo == null){
            throw new ArgumentException($"El tipo de pruducto con el id: {id}, no existe");
        }
        
        var existeNombretipo = await repository.ExisteNombre(tipoDto.Nombre,id);
        if (existeNombretipo){
            throw new ArgumentException($"Ya existe un tipo de pruducto con el nombre {tipoDto.Nombre}");
        }

        //Mapeo Dto => Entidad
        tipo = mapper.Map<TipoProductoActualizarDto, TipoProducto> (tipoDto, tipo);

        //Persistencia objeto
        await repository.UpdateAsync(tipo);
        await repository.UnitOfWork.SaveChangesAsync();

        return;
    }

    public async Task<bool> DeleteAsync(string tipoId)
    {
        //Reglas Validaciones... 
        var tipo = await repository.GetByIdAsync(tipoId);
        if (tipo == null){
            throw new ArgumentException($"El tipo de pruducto con el id: {tipoId}, no existe");
        }

        repository.Delete(tipo);
        await repository.UnitOfWork.SaveChangesAsync();

        return true;
    }

    public ICollection<TipoProductoDto> GetAll()
    {
        var tipoList = repository.GetAll();

        var tipoListDto =  from m in tipoList
                            select new TipoProductoDto(){
                                Id = m.Id,
                                Nombre = m.Nombre
                            };

        return tipoListDto.ToList();
    }

}
 