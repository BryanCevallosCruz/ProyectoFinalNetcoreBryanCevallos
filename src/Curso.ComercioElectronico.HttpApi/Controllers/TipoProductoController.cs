

using Curso.ComercioElectronico.Application;
using Microsoft.AspNetCore.Mvc;

namespace Curso.ComercioElectronico.HttpApi.Controllers;


[ApiController]
[Route("[controller]")]
public class TipoProductoController : ControllerBase
{

    private readonly ITipoProductoAppService tipoAppService;

    public TipoProductoController(ITipoProductoAppService tipoAppService)
    {
        this.tipoAppService = tipoAppService;
    }

    [HttpGet]
    public ICollection<TipoProductoDto> GetAll()
    {

        return tipoAppService.GetAll();
    }

    [HttpPost]
    public async Task<TipoProductoDto> CreateAsync(TipoProductoCrearDto tipo)
    {

        return await tipoAppService.CreateAsync(tipo);

    }

    [HttpPut]
    public async Task UpdateAsync(string id, TipoProductoActualizarDto tipo)
    {

        await tipoAppService.UpdateAsync(id, tipo);

    }

    [HttpDelete]
    public async Task<bool> DeleteAsync(string tipoId)
    {

        return await tipoAppService.DeleteAsync(tipoId);

    }

}