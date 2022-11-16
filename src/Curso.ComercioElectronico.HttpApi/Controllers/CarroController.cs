using Curso.ComercioElectronico.Application;
using Microsoft.AspNetCore.Mvc;

namespace Curso.ComercioElectronico.HttpApi.Controllers;


[ApiController]
[Route("[controller]")]
public class CarroController : ControllerBase
{

    private readonly ICarroAppService carroAppService;


    public CarroController(ICarroAppService carroAppService)
    {
        this.carroAppService = carroAppService;
    }

    [HttpGet]
    public Task<ListaPaginada<CarroDto>> GetAll([FromQuery]CarroListInput input)
    {
        return carroAppService.GetAll(input);
    }

    [HttpGet("{id}")]
    public async Task<CarroDto>  GetByIdAsync(Guid id)
    {
        return await carroAppService.GetByIdAsync(id);
    }


    [HttpPost]
    public async Task<CarroDto> CreateAsync(CarroCrearDto carro)
    {
        return await carroAppService.CreateAsync(carro);
    }

    [HttpPut]
    public async Task UpdateAsync(Guid id, CarroActualizarDto carro)
    {
        await carroAppService.UpdateAsync(id, carro);
    }

    [HttpDelete]
    public async Task<bool> DeleteAsync(Guid carroId)
    {
        return await carroAppService.DeleteAsync(carroId);
    }

    // Carro Item *******************
    [HttpGet("items")]
    public ListaPaginada<CarroItemDto> GetAllItem(int limit=10,int offset=0)
    {
        return carroAppService.GetAllItem(limit,offset);
    }


    [HttpGet("item/{id}")]
    public async Task<CarroItemDto>  GetByIdAsyncItem(Guid id)
    {
        return await carroAppService.GetByIdAsyncItem(id);
    }


    [HttpPost("nuevo-item")]
    public async Task<CarroItemDto> CreateAsyncItem(Guid carroId, CarroItemCrearDto carro)
    {
        return await carroAppService.CreateAsyncItem(carroId, carro);
    }

    [HttpPut("observaciones-item")]
    public async Task UpdateAsyncItem(Guid itemId, CarroItemActualizarDto carroItem)
    {
        await carroAppService.UpdateAsyncItem(itemId, carroItem);
    }

    [HttpDelete("item")]
    public async Task<bool> DeleteAsyncItem(Guid carroItemId)
    {
        return await carroAppService.DeleteAsync(carroItemId);
    }

}