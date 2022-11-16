using AutoMapper;
using Curso.ComercioElectronico.Domain;

namespace Curso.ComercioElectronico.Application;

public class ClienteAppService : IClienteAppService
{
    private readonly IClienteRepository clienteRepository;
    private readonly IBancoClienteRepository bancoClienteRepository;
    private readonly IMapper mapper;

    public ClienteAppService(IClienteRepository clienteRepository,
            IBancoClienteRepository bancoClienteRepository,
            IMapper mapper)
    {
        this.clienteRepository = clienteRepository;
        this.bancoClienteRepository = bancoClienteRepository;
        this.mapper = mapper;
    }

    public async Task<ClienteDto> CreateAsync(ClienteCrearDto clienteDto)
    {
        //Mapeo Dto => Entidad
        var cliente = mapper.Map<Cliente>(clienteDto);

        //Persistencia objeto
        cliente = await clienteRepository.AddAsync(cliente);
        await clienteRepository.UnitOfWork.SaveChangesAsync();

        //Mapeo Entidad => Dto
        // var clienteCreado = mapper.Map<ClienteDto>(cliente);
        // return clienteCreado;
        return await GetByIdAsync(cliente.Id);
    }

    public async Task<bool> DeleteAsync(Guid clienteId)
    {
        //Reglas Validaciones... 
        var cliente = await clienteRepository.GetByIdAsync(clienteId);
        if (cliente == null)
        {
            throw new ArgumentException($"El cliente con el id: {clienteId}, no existe");
        }

        clienteRepository.Delete(cliente);
        await clienteRepository.UnitOfWork.SaveChangesAsync();
        return true;
    }

    public ListaPaginada<ClienteDto> GetAll(int limit = 10, int offset = 0)
    {
        // Lista
        var consulta = clienteRepository.GetAllIncluding(x => x.BancoCliente);
        // Ejecutar linq
        var total = consulta.Count();
        // Obtener el listado paginado
        consulta = consulta.Skip(offset)
                            .Take(limit)
                            .Select(x => x);
        
        var clientesLista = consulta.ToList();

        // Mapeo de Cliente a Coleccion de ClienteDto
        var listaClientesDto = mapper.Map<ICollection<ClienteDto>>(clientesLista);
        
        var resultado = new ListaPaginada<ClienteDto>();
        resultado.Total = total;
        resultado.Lista = listaClientesDto.ToList();
   
        return resultado;
        
    }

    public async Task<ClienteDto> GetByIdAsync(Guid id)
    {
        var consulta = clienteRepository.GetAllIncluding(x => x.BancoCliente)
                                        .Where(x => x.Id == id)
                                        .Select(x => x);
        var cliente = consulta.SingleOrDefault();
        // Mapeo de Cliente a ClienteDto
        return mapper.Map<ClienteDto>(cliente);
    }

    public async Task<ListaPaginada<ClienteDto>> GetListAsync(ClienteListInput input)
    {
        var consulta = clienteRepository.GetAllIncluding(x => x.BancoCliente);

        //Aplicar filtros
        if (input.BancoId != null)
        {
            consulta = consulta.Where(x => x.BancoClienteId == input.BancoId);
        }

        if (input.BancoId != null)
        {
            consulta = consulta.Where(x => x.BancoClienteId == input.BancoId);
        }

        if (!string.IsNullOrEmpty(input.NombreBuscar))
        {
            consulta = consulta.Where(x => x.Nombre.Contains(input.NombreBuscar));
        }

        if (input.Cedula != null)
        {
            consulta = consulta.Where(x => x.Cedula == input.Cedula);
        }

        //Ejecuatar linq. Total registros
        var total = consulta.Count();

        //Aplicar paginacion
        consulta = consulta.Skip(input.Offset)
                    .Take(input.Limit)
                    .Select(x => x);
        var clientesLista = consulta.ToList();

        // Mapeo de Cliente a Coleccion de ClienteDto
        var listaClientesDto = mapper.Map<ICollection<ClienteDto>>(clientesLista);
      
        var resultado = new ListaPaginada<ClienteDto>();
        resultado.Total = total;
        resultado.Lista = listaClientesDto.ToList();
   
        return resultado;
    }

    public async Task UpdateAsync(Guid id, ClienteActualizarDto clienteDto)
    {
        var cliente = await clienteRepository.GetByIdAsync(id);
        if (cliente == null)
        {
            throw new ArgumentException($"El cliente con el id: {id}, no existe");
        }

        var existeNombreCliente = await clienteRepository.ExisteNombre(clienteDto.Nombre, id);
        if (existeNombreCliente)
        {
            throw new ArgumentException($"Ya existe un cliente con el nombre {clienteDto.Nombre}");
        }

        //Mapeo Dto => Entidad

        cliente = mapper.Map<ClienteActualizarDto, Cliente>(clienteDto, cliente);

        //Persistencia objeto
        await clienteRepository.UpdateAsync(cliente);
        await clienteRepository.UnitOfWork.SaveChangesAsync();

        return;
    }
}