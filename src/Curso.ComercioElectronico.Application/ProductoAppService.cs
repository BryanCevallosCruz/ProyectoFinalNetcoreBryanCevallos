using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Curso.ComercioElectronico.Domain;

namespace Curso.ComercioElectronico.Application;

public class ProductoAppService : IProductoAppService
{
    private readonly IProductoRepository productoRepository;
    private readonly IMarcaRepository marcaRepository;
    private readonly ITipoProductoRepository tipoProductoRepository;
    private readonly IMapper mapper;
    public ProductoAppService(IProductoRepository productoRepository,
        IMarcaRepository marcaRepository,
        ITipoProductoRepository tipoProductoRepository,
        IMapper mapper)
    {
        this.productoRepository = productoRepository;
        this.marcaRepository = marcaRepository;
        this.tipoProductoRepository = tipoProductoRepository;
        this.mapper = mapper;
    }
    public async Task<ProductoDto> CreateAsync(ProductoCrearActualizarDto productoDto)
    {
        //Mapeo Dto => Entidad
        var producto = mapper.Map<Producto>(productoDto);

        //Persistencia objeto
        producto = await productoRepository.AddAsync(producto);
        await productoRepository.UnitOfWork.SaveChangesAsync();

        return await GetByIdAsync(producto.Id);

    }

    public async Task<bool> DeleteAsync(Guid productoId)
    {
         //Reglas Validaciones... 
        var producto = await productoRepository.GetByIdAsync(productoId);
        if (producto == null)
        {
            throw new ArgumentException($"El producto con el id: {productoId}, no existe");
        }

        productoRepository.Delete(producto);
        await productoRepository.UnitOfWork.SaveChangesAsync();
        return true;
    }

    public ListaPaginada<ProductoDto> GetAll(int limit = 10, int offset = 0)
    {
         // Lista
        var consulta = productoRepository.GetAllIncluding(x => x.Marca, x => x.TipoProducto);
        // Ejecutar linq
        var total = consulta.Count();
        // Obtener el listado paginado
        consulta = consulta.Skip(offset)
                            .Take(limit)
                            .Select(x => x);
        
        var productosLista = consulta.ToList();

        // Mapeo de Producto a Coleccion de ProductoDto
        var listaProductoDto = mapper.Map<ICollection<ProductoDto>>(productosLista);
        
        var resultado = new ListaPaginada<ProductoDto>();
        resultado.Total = total;
        resultado.Lista = listaProductoDto.ToList();
   
        return resultado;
    }

    public async Task<ProductoDto> GetByIdAsync(Guid id)
    {
        var consulta = productoRepository.GetAllIncluding(x => x.Marca, x => x.TipoProducto)
                                        .Where(x => x.Id == id)
                                        .Select(x => x);
        var producto = consulta.SingleOrDefault();
        // Mapeo de Producto a ProductoDto
        return mapper.Map<ProductoDto>(producto);
        
        
    }

    public async Task<ListaPaginada<ProductoDto>> GetListAsync(ProductoListInput input)
    {
        var consulta = productoRepository.GetAllIncluding(x => x.Marca,
                            x => x.TipoProducto);
  
        //Aplicar filtros
        if (input.TipoProductoId != null){
          consulta = consulta.Where(x => x.TipoProductoId == input.TipoProductoId);
        }

        if (input.MarcaId != null){
          consulta = consulta.Where(x => x.MarcaId == input.MarcaId);
        }

        if (!string.IsNullOrEmpty(input.NombreProducto)){
            consulta = consulta.Where(x => x.Nombre.Contains(input.NombreProducto));
        }

        //Ejecutar linq. Total registros
        var total = consulta.Count();

        //Aplicar paginacion
        consulta = consulta.Skip(input.Offset)
                    .Take(input.Limit)
                    .Select(x => x);
        var productosLista = consulta.ToList();

        // Mapeo de Producto a Coleccion de ProductoDto
        var listaProductosDto = mapper.Map<ICollection<ProductoDto>>(productosLista);
      
        var resultado = new ListaPaginada<ProductoDto>();
        resultado.Total = total;
        resultado.Lista = listaProductosDto.ToList();
   
        return resultado;
    }

    public async Task UpdateAsync(Guid id, ProductoCrearActualizarDto productoDto)
    {
        var producto = await productoRepository.GetByIdAsync(id);
        if (producto == null)
        {
            throw new ArgumentException($"El producto con el id: {id}, no existe");
        }

        var existeNombreProducto = await productoRepository.ExisteNombre(productoDto.Nombre, id);
        if (existeNombreProducto)
        {
            throw new ArgumentException($"Ya existe un producto con el nombre {productoDto.Nombre}");
        }

        //Mapeo Dto => Entidad
        //producto.Nombre = productoDto.Nombre;
        producto = mapper.Map<ProductoCrearActualizarDto, Producto>(productoDto, producto);

        //Persistencia objeto
        await productoRepository.UpdateAsync(producto);
        await productoRepository.UnitOfWork.SaveChangesAsync();

        return;
    }
}
