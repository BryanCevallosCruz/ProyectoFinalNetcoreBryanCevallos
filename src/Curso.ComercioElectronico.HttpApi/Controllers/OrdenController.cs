

using Curso.ComercioElectronico.Application;
using Microsoft.AspNetCore.Mvc;

namespace Curso.ComercioElectronico.HttpApi.Controllers;


[ApiController]
[Route("[controller]")]
public class OrdenController : ControllerBase
{

   
    private readonly IOrdenAppService ordenAppService;


    public OrdenController(IOrdenAppService ordenAppService)
    {
        this.ordenAppService = ordenAppService;
    }

    [HttpGet]
    public Task<ListaPaginada<OrdenDto>> GetAll([FromQuery]OrdenListInput input)
    {
        return ordenAppService.GetAll(input);
    }

    [HttpGet("{id}")]
    public async Task<OrdenDto>  GetByIdAsync(Guid id)
    {
        return await ordenAppService.GetByIdAsync(id);
    }


    [HttpPost]
    public async Task<OrdenDto> CreateAsync(Guid carroId, OrdenCrearDto ordenDto)
    {
        return await ordenAppService.CreateAsync(carroId, ordenDto);
    }

    [HttpPut]
    public async Task UpdateAsync(Guid id, OrdenActualizarDto orden)
    {
        await ordenAppService.UpdateAsync(id, orden);
    }

    [HttpDelete]
    public async Task<bool> DeleteAsync(Guid ordenId)
    {
        return await ordenAppService.DeleteAsync(ordenId);
    }

    // Orden Item *******************
    [HttpGet("items")]
    public ListaPaginada<OrdenItemDto> GetAllItem(int limit=10,int offset=0)
    {
        return ordenAppService.GetAllItem(limit,offset);
    }


    [HttpGet("item/{id}")]
    public async Task<OrdenItemDto>  GetByIdAsyncItem(Guid id)
    {
        return await ordenAppService.GetByIdAsyncItem(id);
    }


    [HttpPost("nuevo-item")]
    public async Task<OrdenItemDto> CreateAsyncItem(Guid ordenId, OrdenItemCrearDto orden)
    {
        return await ordenAppService.CreateAsyncItem(ordenId, orden);
    }

    [HttpPut("observaciones-item")]
    public async Task UpdateAsyncItem(Guid itemId, OrdenItemActualizarDto ordenItem)
    {
        await ordenAppService.UpdateAsyncItem(itemId, ordenItem);
    }

    [HttpDelete("item")]
    public async Task<bool> DeleteAsyncItem(Guid ordenItemId)
    {
        return await ordenAppService.DeleteAsync(ordenItemId);
    }

}