

using Curso.ComercioElectronico.Application;
using Microsoft.AspNetCore.Mvc;

namespace Curso.ComercioElectronico.HttpApi.Controllers;


[ApiController]
[Route("[controller]")]
public class ProductoController : ControllerBase
{

    private readonly IProductoAppService productoAppService;

    public ProductoController(IProductoAppService productoAppService)
    {
        this.productoAppService = productoAppService;
    }

    [HttpGet]
    public ListaPaginada<ProductoDto> GetAll(int limit=10,int offset=0)
    {

        return productoAppService.GetAll(limit,offset);

    }

    [HttpGet("list")]
    public  Task<ListaPaginada<ProductoDto>> GetListAsync([FromQuery]ProductoListInput input)
    {
        return productoAppService.GetListAsync(input);
    }

    [HttpGet("{id}")]
    public async Task<ProductoDto>  GetByIdAsync(Guid id)
    {
        return await productoAppService.GetByIdAsync(id);
    }


    

    [HttpPost]
    public async Task<ProductoDto> CreateAsync(ProductoCrearActualizarDto marca)
    {

        return await productoAppService.CreateAsync(marca);

    }

    [HttpPut]
    public async Task UpdateAsync(Guid id, ProductoCrearActualizarDto marca)
    {

        await productoAppService.UpdateAsync(id, marca);

    }

    [HttpDelete]
    public async Task<bool> DeleteAsync(Guid marcaId)
    {

        return await productoAppService.DeleteAsync(marcaId);

    }

}