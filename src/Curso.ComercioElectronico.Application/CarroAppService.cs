using AutoMapper;
using Curso.ComercioElectronico.Domain;
namespace Curso.ComercioElectronico.Application;

public class CarroAppService : ICarroAppService
{
    private readonly ICarroRepository carroRepository;
    private readonly ICarroItemRepository carroItemRepository;
    private readonly IProductoRepository productoRepository;
    private readonly IProductoAppService productoAppService;
    private readonly IMapper mapper;

    public CarroAppService(ICarroRepository carroRepository,
            ICarroItemRepository carroItemRepository,
            IProductoRepository productoRepository,
            IProductoAppService productoAppService,
            IMapper mapper)
    {
        this.carroRepository = carroRepository;
        this.carroItemRepository = carroItemRepository;
        this.productoRepository = productoRepository;
        this.productoAppService = productoAppService;
        this.mapper = mapper;
    }
    public async Task<CarroDto> CreateAsync(CarroCrearDto carroDto)
    {
         //2. Mapeos
        var carro = new Carro();
        carro.ClienteId = carroDto.ClienteId;
        carro.Estado = CarroEstado.EnProceso;
        carro.Fecha = carroDto.Fecha; 

        foreach (var item in carroDto.Items)
        {
            var productoDto = await productoAppService.GetByIdAsync(item.ProductoId);
            if (productoDto != null){
                var carroItem = new CarroItem();
                carroItem.Cantidad = item.Cantidad;
                carroItem.Precio = productoDto.Precio;
                carroItem.ProductoId = productoDto.Id;
                carroItem.Observaciones = item.Observaciones;
                carro.AgregarItem(carroItem);
            }
        }
        
        carro.Total =  carro.Items.Sum(x => x.Cantidad*x.Precio);

        //3. Persistencias.
        carro = await carroRepository.AddAsync(carro);
        await carroRepository.UnitOfWork.SaveChangesAsync();
        
        return await GetByIdAsync(carro.Id);
        
    }

    public async Task<bool> DeleteAsync(Guid carroId)
    {
        //Reglas Validaciones... 
        var carro = await carroRepository.GetByIdAsync(carroId);
        if (carro == null)
        {
            throw new ArgumentException($"El carro con el id: {carroId}, no existe");
        }

        carroRepository.Delete(carro);
        await carroRepository.UnitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<ListaPaginada<CarroDto>> GetAll(CarroListInput input)
    {
        // Lista
        var consulta = carroRepository.GetAllIncluding(x => x.Cliente, x => x.Items);
  
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
        
        var carrosLista = consulta.ToList();

        // Mapeo de Carro a Coleccion de CarroDto
        var listaCarrosDto = mapper.Map<ICollection<CarroDto>>(carrosLista);
        
        var resultado = new ListaPaginada<CarroDto>();
        resultado.Total = total;
        resultado.Lista = listaCarrosDto.ToList();
   
        return resultado;
    }

    public Task<CarroDto> GetByIdAsync(Guid id)
    {

        var consulta = carroRepository.GetAllIncluding(x => x.Cliente, x => x.Items); //, x => x.Vendedor);
        consulta = consulta.Where(x => x.Id == id);

        var consultaCarroDto = consulta
                                .Select(
                                    x => new CarroDto()
                                    {
                                         Id = x.Id,
                                         Cliente = x.Cliente.Nombre,
                                         ClienteId = x.ClienteId,
                                         Estado = x.Estado,
                                         Fecha = x.Fecha,
                                         Total = x.Total,
                                         Items = x.Items.Select(item => new CarroItemDto(){
                                            Cantidad = item.Cantidad,
                                            Id = item.Id,
                                            Observaciones = item.Observaciones,
                                            CarroId = item.CarroId,
                                            Precio  = item.Precio,
                                            ProductoId = item.ProductoId,
                                            Producto = item.Producto.Nombre
                                         }).ToList()
                                    }
                                ); 
        return Task.FromResult(consultaCarroDto.SingleOrDefault());
    }

    public async Task UpdateAsync(Guid id, CarroActualizarDto carroDto)
    {
        var carro = await carroRepository.GetByIdAsync(id);
        if (carro == null)
        {
            throw new ArgumentException($"El carro con el id: {id}, no existe");
        }

        //Mapeo Dto => Entidad

        carro = mapper.Map<CarroActualizarDto, Carro>(carroDto, carro);

        //Persistencia objeto
        await carroRepository.UpdateAsync(carro);
        await carroRepository.UnitOfWork.SaveChangesAsync();

        return;
    }

    // Para CarroItem *****************
     public async Task<CarroItemDto> CreateAsyncItem(Guid id, CarroItemCrearDto carroItemDto)
    {
        var carro = await carroRepository.GetByIdAsync(id);
        if (carro == null)
        {
            throw new ArgumentException($"El carro con el id: {id}, no existe");
        }
        
        //Mapeo Dto => Entidad
        var carroItem = mapper.Map<CarroItem>(carroItemDto);
        //Mapeo manual de las propiedades faltantes
        carroItem.CarroId = id;
        var productoDto = await productoAppService.GetByIdAsync(carroItem.ProductoId);
        carroItem.Precio = productoDto.Precio;
        //Persistencia objeto
        carroItem = await carroItemRepository.AddAsync(carroItem);
        await carroItemRepository.UnitOfWork.SaveChangesAsync();

        carro.AgregarItem(carroItem);
        carro.Total = carro.Total + carroItem.Precio*carroItem.Cantidad;
        //carro.Total = carro.Items.Sum(x =>  x.Cantidad*x.Precio);
        await carroRepository.UnitOfWork.SaveChangesAsync();

        //Mapeo Entidad => Dto
        //var carroCreado = mapper.Map<CarroItemDto>(carroItem);
  
        return await GetByIdAsyncItem(carroItem.Id);
    }

    public async Task<bool> DeleteAsyncItem(Guid carroItemId)
    {
        //Reglas Validaciones... 
        var carroItem = await carroItemRepository.GetByIdAsync(carroItemId);
        
        if (carroItem == null)
        {
            throw new ArgumentException($"El item con el id: {carroItemId}, no existe");
        }
        var carro = await carroRepository.GetByIdAsync(carroItem.CarroId);
        carro.Total = carro.Total - carroItem.Precio*carroItem.Cantidad;
        // Eliminar
        carroItemRepository.Delete(carroItem);
        //Guardar
        await carroItemRepository.UnitOfWork.SaveChangesAsync();

        await carroRepository.UnitOfWork.SaveChangesAsync();

        return true;
    }

    public ListaPaginada<CarroItemDto> GetAllItem(int limit = 10, int offset = 0)
    {
        // Lista
        var consulta = carroItemRepository.GetAllIncluding(x => x.Producto);
        // Ejecutar linq
        var total = consulta.Count();
        // Obtener el listado paginado
        consulta = consulta.Skip(offset)
                            .Take(limit)
                            .Select(x => x);
        
        var carrosLista = consulta.ToList();

        // Mapeo de Carro a Coleccion de CarroDto
        var listaCarrosDto = mapper.Map<ICollection<CarroItemDto>>(carrosLista);
        
        var resultado = new ListaPaginada<CarroItemDto>();
        resultado.Total = total;
        resultado.Lista = listaCarrosDto.ToList();
   
        return resultado;
    }

    public async Task<CarroItemDto> GetByIdAsyncItem(Guid id)
    {
        var carroExiste = await carroItemRepository.GetByIdAsync(id);
        if (carroExiste == null)
        {
            throw new ArgumentException($"El item con el id: {id}, no existe");
        }
        var consulta = carroItemRepository.GetAllIncluding(x => x.Producto)
                                        .Where(x => x.Id == id)
                                        .Select(x => x);
        var carro = consulta.SingleOrDefault();
        // Mapeo de Carro a CarroDto
        return mapper.Map<CarroItemDto>(carro);
    }

    public async Task UpdateAsyncItem(Guid id, CarroItemActualizarDto carroItemDto)
    {
        var carro = await carroItemRepository.GetByIdAsync(id);
        if (carro == null)
        {
            throw new ArgumentException($"El item con el id: {id}, no existe");
        }

        //Mapeo Dto => Entidad

        carro = mapper.Map<CarroItemActualizarDto, CarroItem>(carroItemDto, carro);

        //Persistencia objeto
        await carroItemRepository.UpdateAsync(carro);
        await carroItemRepository.UnitOfWork.SaveChangesAsync();

        return;
    }


}
