

using Curso.ComercioElectronico.Application;
using Microsoft.AspNetCore.Mvc;

namespace Curso.ComercioElectronico.HttpApi.Controllers;


[ApiController]
[Route("[controller]")]
public class BancoClienteController : ControllerBase
{

    private readonly IBancoClienteAppService bancoAppService;

    public BancoClienteController(IBancoClienteAppService bancoAppService)
    {
        this.bancoAppService = bancoAppService;
    }

    [HttpGet]
    public ICollection<BancoClienteDto> GetAll()
    {

        return bancoAppService.GetAll();
    }

    [HttpPost]
    public async Task<BancoClienteDto> CreateAsync(BancoClienteCrearDto banco)
    {

        return await bancoAppService.CreateAsync(banco);

    }

    [HttpPut]
    public async Task UpdateAsync(string id, BancoClienteActualizarDto banco)
    {

        await bancoAppService.UpdateAsync(id, banco);

    }

    [HttpDelete]
    public async Task<bool> DeleteAsync(string bancoId)
    {

        return await bancoAppService.DeleteAsync(bancoId);

    }

}