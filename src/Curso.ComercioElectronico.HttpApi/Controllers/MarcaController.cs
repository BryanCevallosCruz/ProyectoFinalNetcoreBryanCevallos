

using Curso.ComercioElectronico.Application;
using Microsoft.AspNetCore.Mvc;

namespace Curso.ComercioElectronico.HttpApi.Controllers;


[ApiController]
[Route("[controller]")]
public class MarcaController : ControllerBase
{

    private readonly IMarcaAppService marcaAppService;

    public MarcaController(IMarcaAppService marcaAppService)
    {
        this.marcaAppService = marcaAppService;
    }

    [HttpGet]
    public ICollection<MarcaDto> GetAll()
    {

        return marcaAppService.GetAll();
    }

    [HttpPost]
    public async Task<MarcaDto> CreateAsync(MarcaCrearDto marca)
    {

        return await marcaAppService.CreateAsync(marca);

    }

    [HttpPut]
    public async Task UpdateAsync(string id, MarcaActualizarDto marca)
    {

        await marcaAppService.UpdateAsync(id, marca);

    }

    [HttpDelete]
    public async Task<bool> DeleteAsync(string marcaId)
    {

        return await marcaAppService.DeleteAsync(marcaId);

    }

}