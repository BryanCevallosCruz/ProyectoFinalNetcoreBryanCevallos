using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Curso.ComercioElectronico.Domain;
using Microsoft.Extensions.Logging;

namespace Curso.ComercioElectronico.Application;

public class BancoClienteAppService : IBancoClienteAppService
{
    private readonly IBancoClienteRepository repository;
    private readonly IMapper mapper;
    private readonly ILogger<BancoClienteAppService> logger;

    public BancoClienteAppService(IBancoClienteRepository repository,
            IMapper mapper, ILogger<BancoClienteAppService> logger)
    {
        this.repository = repository;
        this.mapper = mapper;
        this.logger = logger;
    }

    public async Task<BancoClienteDto> CreateAsync(BancoClienteCrearDto bancoClienteDto)
    {

        logger.LogInformation("Crear Banco Cliente");

        //Reglas Validaciones... 
        var existeNombreBanco = await repository.ExisteNombre(bancoClienteDto.Nombre);
        if (existeNombreBanco)
        {
            var msg = $"Ya existe un banco con el nombre {bancoClienteDto.Nombre}";
            logger.LogError(msg);

            throw new ArgumentException(msg);
        }


        //Mapeo Dto => Entidad. 
        var bancoCliente = mapper.Map<BancoCliente>(bancoClienteDto);

        //Persistencia objeto
        bancoCliente = await repository.AddAsync(bancoCliente);
        await repository.UnitOfWork.SaveChangesAsync();

        //Mapeo Entidad => Dto
        var bancoClienteCreado = mapper.Map<BancoClienteDto>(bancoCliente);


        return bancoClienteCreado;
    }

    public ICollection<BancoClienteDto> GetAll()
    {
        var bancoClienteList = repository.GetAll();

        var bancoClienteListDto = from m in bancoClienteList
                                  select new BancoClienteDto()
                                  {
                                      Id = m.Id,
                                      Nombre = m.Nombre
                                  };

        return bancoClienteListDto.ToList();
    }


    public async Task UpdateAsync(string id, BancoClienteActualizarDto bancoClienteDto)
    {
        logger.LogInformation("Actualizar Banco Cliente");

        //Reglas Validaciones... 
        var bancoCliente = await repository.GetByIdAsync(id);
        if (bancoCliente == null){
            throw new ArgumentException($"El Banco con el id: {id}, no existe");
        }
        
        var existeNombreBanco = await repository.ExisteNombre(bancoClienteDto.Nombre, id);
        if (existeNombreBanco)
        {
            var msg = $"Ya existe un banco con el nombre {bancoClienteDto.Nombre}";
            logger.LogError(msg);

            throw new ArgumentException(msg);
        }


        // mapeo manual
        //Mapeo Dto => Entidad
        // bancoCliente.Nombre = bancoClienteDto.Nombre;

        // //Persistencia objeto
        // await repository.UpdateAsync(bancoCliente);
        // await repository.UnitOfWork.SaveChangesAsync();
        // ********************************************
        // Mapeo automatico
        //Mapeo Dto => Entidad. 
        // TDestination Map<TSource, TDestination>(TSource source);
        bancoCliente = mapper.Map<BancoClienteActualizarDto, BancoCliente>(bancoClienteDto, bancoCliente); // explicita
        // mapper.Map(bancoClienteDto, bancoCliente); //Implicita igual a la explicita
        //Persistencia objeto
        await repository.UpdateAsync(bancoCliente);
        await repository.UnitOfWork.SaveChangesAsync();

        // //Mapeo Entidad => Dto
        // // var bancoClienteCreado = mapper.Map<BancoClienteDto>(bancoCliente);


        return;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        //Reglas Validaciones... 
        var bancoCliente = await repository.GetByIdAsync(id);
        if (bancoCliente == null)
        {
            throw new ArgumentException($"El Banco con el id: {id}, no existe");
        }

        repository.Delete(bancoCliente);
        await repository.UnitOfWork.SaveChangesAsync();

        return true;
    }

}