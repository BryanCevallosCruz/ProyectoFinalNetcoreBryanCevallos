using AutoMapper;
using Curso.ComercioElectronico.Domain;
using Microsoft.Extensions.Logging;

namespace Curso.ComercioElectronico.Application;

public class OrdenAppService : IOrdenAppService
{
    private readonly IOrdenRepository ordenRepository;
    private readonly IOrdenItemRepository ordenItemRepository;
    private readonly IProductoAppService productoAppService;
    private readonly ICarroRepository carroRepository;
    private readonly ILogger<OrdenAppService> logger;
    private readonly IMapper mapper;
    private readonly IClienteRepository clienteRepository;

    public OrdenAppService(
        IOrdenRepository ordenRepository,
        IOrdenItemRepository ordenItemRepository,
        //IProductoRepository productoRepository,
        IProductoAppService productoAppService,
        ICarroRepository carroRepository,
        ILogger<OrdenAppService> logger,
        IMapper mapper,
        IClienteRepository clienteRepository)
    {
        this.ordenRepository = ordenRepository;
        this.ordenItemRepository = ordenItemRepository;
        this.productoAppService = productoAppService;
        this.carroRepository = carroRepository;
        this.logger = logger;
        this.mapper = mapper;
        this.clienteRepository = clienteRepository;
    }

    public async Task<OrdenDto> CreateAsync(Guid carroId, OrdenCrearDto ordenCrearDto)
    {
        
        var carro = await carroRepository.GetByIdAsync(carroId);

        if (carro == null)
        {
            throw new ArgumentException($"El carro con el id: {carroId}, no existe");
        }
        //Mapeos


        var ordenDto = mapper.Map<OrdenDto>(carro);
        ordenDto.Estado = OrdenEstado.EnProceso;
        ordenDto.Fecha = ordenCrearDto.Fecha; 
        ordenDto.Observaciones = ordenCrearDto.Observaciones;
        var orden = mapper.Map<Orden>(ordenDto);

    

        var cliente = await clienteRepository.GetByIdAsync(orden.ClienteId);
        if (cliente.PorcentajeDescuento > 0 ){
            orden.Observaciones+=$"El cliente tiene un descuento del {cliente.PorcentajeDescuento} %.";
            orden.Total = orden.Total*(100-cliente.PorcentajeDescuento)/100;
        }
        //3. Persistencias.
        
        orden = await ordenRepository.AddAsync(orden);
        await ordenRepository.UnitOfWork.SaveChangesAsync();
        
        return await GetByIdAsync(orden.Id);
        
    }

    public async Task<bool> DeleteAsync(Guid ordenId)
    {
        //Reglas Validaciones... 
        var orden = await ordenRepository.GetByIdAsync(ordenId);
        if (orden == null)
        {
            throw new ArgumentException($"El orden con el id: {ordenId}, no existe");
        }

        ordenRepository.Delete(orden);
        await ordenRepository.UnitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<ListaPaginada<OrdenDto>> GetAll(OrdenListInput input)
    {
        // Lista
        var consulta = ordenRepository.GetAllIncluding(x => x.Cliente, x => x.Items);
  
        //Aplicar filtros
        if (input.ClienteId != null){
          consulta = consulta.Where(x => x.ClienteId == input.ClienteId);
        }

        if (!string.IsNullOrEmpty(input.NombreCliente)){
            consulta = consulta.Where(x => x.Cliente.Nombre.Contains(input.NombreCliente));
        }

        
        // Ejecutar linq
        var total = consulta.Count();
        // Obtener el listado paginado
        consulta = consulta.Skip(input.Offset)
                            .Take(input.Limit)
                            .Select(x => x);
        
        var ordensLista = consulta.ToList();

        // Mapeo de Orden a Coleccion de OrdenDto
        var listaOrdensDto = mapper.Map<ICollection<OrdenDto>>(ordensLista);
        
        var resultado = new ListaPaginada<OrdenDto>();
        resultado.Total = total;
        resultado.Lista = listaOrdensDto.ToList();
   
        return resultado;
    }

    public Task<OrdenDto> GetByIdAsync(Guid id)
    {

        var consulta = ordenRepository.GetAllIncluding(x => x.Cliente, x => x.Items); //, x => x.Vendedor);
        consulta = consulta.Where(x => x.Id == id);

        var consultaOrdenDto = consulta
                                .Select(
                                    x => new OrdenDto()
                                    {
                                         Id = x.Id,
                                         Cliente = x.Cliente.Nombre,
                                         ClienteId = x.ClienteId,
                                         Estado = x.Estado,
                                         Fecha = x.Fecha,
                                         Total = x.Total,
                                         Observaciones = x.Observaciones,
                                         Items = x.Items.Select(item => new OrdenItemDto(){
                                            Cantidad = item.Cantidad,
                                            Id = item.Id,
                                            Observaciones = item.Observaciones,
                                            OrdenId = item.OrdenId,
                                            Precio  = item.Precio,
                                            ProductoId = item.ProductoId,
                                            Producto = item.Producto.Nombre
                                         }).ToList()
                                    }
                                ); 
        return Task.FromResult(consultaOrdenDto.SingleOrDefault());
    }

    public async Task UpdateAsync(Guid id, OrdenActualizarDto ordenDto)
    {
        var orden = await ordenRepository.GetByIdAsync(id);
        if (orden == null)
        {
            throw new ArgumentException($"El orden con el id: {id}, no existe");
        }

        //Mapeo Dto => Entidad

        orden = mapper.Map<OrdenActualizarDto, Orden>(ordenDto, orden);

        //Persistencia objeto
        await ordenRepository.UpdateAsync(orden);
        await ordenRepository.UnitOfWork.SaveChangesAsync();

        return;
    }

    // Para OrdenItem *****************
     public async Task<OrdenItemDto> CreateAsyncItem(Guid id, OrdenItemCrearDto ordenItemDto)
    {
        var orden = await ordenRepository.GetByIdAsync(id);
        if (orden == null)
        {
            throw new ArgumentException($"El orden con el id: {id}, no existe");
        }
        
        //Mapeo Dto => Entidad
        var ordenItem = mapper.Map<OrdenItem>(ordenItemDto);
        //Mapeo manual de las propiedades faltantes
        ordenItem.OrdenId = id;
        var productoDto = await productoAppService.GetByIdAsync(ordenItem.ProductoId);
        ordenItem.Precio = productoDto.Precio;
        //Persistencia objeto
        ordenItem = await ordenItemRepository.AddAsync(ordenItem);
        await ordenItemRepository.UnitOfWork.SaveChangesAsync();

        orden.AgregarItem(ordenItem);
        orden.Total = orden.Total + ordenItem.Precio*ordenItem.Cantidad;
        //orden.Total = orden.Items.Sum(x =>  x.Cantidad*x.Precio);
        await ordenRepository.UnitOfWork.SaveChangesAsync();

        //Mapeo Entidad => Dto
        //var ordenCreado = mapper.Map<OrdenItemDto>(ordenItem);
  
        return await GetByIdAsyncItem(ordenItem.Id);
    }

    public async Task<bool> DeleteAsyncItem(Guid ordenItemId)
    {
        //Reglas Validaciones... 
        var ordenItem = await ordenItemRepository.GetByIdAsync(ordenItemId);
        
        if (ordenItem == null)
        {
            throw new ArgumentException($"El item con el id: {ordenItemId}, no existe");
        }
        var orden = await ordenRepository.GetByIdAsync(ordenItem.OrdenId);
        orden.Total = orden.Total - ordenItem.Precio*ordenItem.Cantidad;
        // Eliminar
        ordenItemRepository.Delete(ordenItem);
        //Guardar
        await ordenItemRepository.UnitOfWork.SaveChangesAsync();

        await ordenRepository.UnitOfWork.SaveChangesAsync();

        return true;
    }

    public ListaPaginada<OrdenItemDto> GetAllItem(int limit = 10, int offset = 0)
    {
        // Lista
        var consulta = ordenItemRepository.GetAllIncluding(x => x.Producto);
        // Ejecutar linq
        var total = consulta.Count();
        // Obtener el listado paginado
        consulta = consulta.Skip(offset)
                            .Take(limit)
                            .Select(x => x);
        
        var ordensLista = consulta.ToList();

        // Mapeo de Orden a Coleccion de OrdenDto
        var listaOrdensDto = mapper.Map<ICollection<OrdenItemDto>>(ordensLista);
        
        var resultado = new ListaPaginada<OrdenItemDto>();
        resultado.Total = total;
        resultado.Lista = listaOrdensDto.ToList();
   
        return resultado;
    }

    public async Task<OrdenItemDto> GetByIdAsyncItem(Guid id)
    {
        var ordenExiste = await ordenItemRepository.GetByIdAsync(id);
        if (ordenExiste == null)
        {
            throw new ArgumentException($"El item con el id: {id}, no existe");
        }
        var consulta = ordenItemRepository.GetAllIncluding(x => x.Producto)
                                        .Where(x => x.Id == id)
                                        .Select(x => x);
        var orden = consulta.SingleOrDefault();
        // Mapeo de Orden a OrdenDto
        return mapper.Map<OrdenItemDto>(orden);
    }

    public async Task UpdateAsyncItem(Guid id, OrdenItemActualizarDto ordenItemDto)
    {
        var orden = await ordenItemRepository.GetByIdAsync(id);
        if (orden == null)
        {
            throw new ArgumentException($"El item con el id: {id}, no existe");
        }

        //Mapeo Dto => Entidad

        orden = mapper.Map<OrdenItemActualizarDto, OrdenItem>(ordenItemDto, orden);

        //Persistencia objeto
        await ordenItemRepository.UpdateAsync(orden);
        await ordenItemRepository.UnitOfWork.SaveChangesAsync();

        return;
    }

}

