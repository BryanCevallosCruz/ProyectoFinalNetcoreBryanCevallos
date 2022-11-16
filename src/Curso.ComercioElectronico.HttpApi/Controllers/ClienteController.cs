
using Curso.ComercioElectronico.Application;
using Microsoft.AspNetCore.Mvc;

namespace Curso.ComercioElectronico.HttpApi.Controllers;


[ApiController]
[Route("[controller]")]
public class ClienteController : ControllerBase
{

    private readonly IClienteAppService clienteAppService;

    public ClienteController(IClienteAppService clienteAppService)
    {
        this.clienteAppService = clienteAppService;
    }

    [HttpGet]
    public ListaPaginada<ClienteDto> GetAll(int limit=10,int offset=0)
    {

        return clienteAppService.GetAll(limit,offset);

    }

    [HttpGet("list")]
    public  Task<ListaPaginada<ClienteDto>> GetListAsync([FromQuery]ClienteListInput input)
    {
        return clienteAppService.GetListAsync(input);
    }

    [HttpGet("{id}")]
    public async Task<ClienteDto>  GetByIdAsync(Guid id)
    {
        return await clienteAppService.GetByIdAsync(id);
    }


    

    [HttpPost]
    public async Task<ClienteDto> CreateAsync(ClienteCrearDto cliente)
    {

        return await clienteAppService.CreateAsync(cliente);

    }

    [HttpPut]
    public async Task UpdateAsync(Guid id, ClienteActualizarDto cliente)
    {

        await clienteAppService.UpdateAsync(id, cliente);

    }

    [HttpDelete]
    public async Task<bool> DeleteAsync(Guid clienteId)
    {

        return await clienteAppService.DeleteAsync(clienteId);

    }

}