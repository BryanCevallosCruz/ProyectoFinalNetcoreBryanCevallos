using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Curso.ComercioElectronico.Domain;

namespace Curso.ComercioElectronico.Application;



public class MarcaAppService : IMarcaAppService
{
    private readonly IMarcaRepository repository;
    private readonly IMapper mapper;

    public MarcaAppService(IMarcaRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<MarcaDto> CreateAsync(MarcaCrearDto marcaDto)
    {
        
        //Reglas Validaciones... 
        var existeNombreMarca = await repository.ExisteNombre(marcaDto.Nombre);
        if (existeNombreMarca){
            throw new ArgumentException($"Ya existe una marca con el nombre {marcaDto.Nombre}");
        }
 
        //Mapeo Dto => Entidad
        var marca = mapper.Map<Marca>(marcaDto);
 
        //Persistencia objeto
        marca = await repository.AddAsync(marca);
        await repository.UnitOfWork.SaveChangesAsync();

        //Mapeo Entidad => Dto
        var marcaCreada = mapper.Map<MarcaDto>(marca);

        return marcaCreada;
    }

    public async Task UpdateAsync(string id, MarcaActualizarDto marcaDto)
    {
        var marca = await repository.GetByIdAsync(id);
        if (marca == null){
            throw new ArgumentException($"La marca con el id: {id}, no existe");
        }
        
        var existeNombreMarca = await repository.ExisteNombre(marcaDto.Nombre,id);
        if (existeNombreMarca){
            throw new ArgumentException($"Ya existe una marca con el nombre {marcaDto.Nombre}");
        }

        //Mapeo Dto => Entidad
        marca = mapper.Map<MarcaActualizarDto, Marca>(marcaDto, marca);
        //Persistencia objeto
        await repository.UpdateAsync(marca);
        await repository.UnitOfWork.SaveChangesAsync();

        return;
    }

    public async Task<bool> DeleteAsync(string marcaId)
    {
        //Reglas Validaciones... 
        var marca = await repository.GetByIdAsync(marcaId);
        if (marca == null){
            throw new ArgumentException($"La marca con el id: {marcaId}, no existe");
        }

        repository.Delete(marca);
        await repository.UnitOfWork.SaveChangesAsync();

        return true;
    }

    public ICollection<MarcaDto> GetAll()
    {
        var marcaList = repository.GetAll();

        var marcaListDto =  from m in marcaList
                            select new MarcaDto(){
                                Id = m.Id,
                                Nombre = m.Nombre
                            };

        return marcaListDto.ToList();
    }

    
}
 