namespace Curso.ComercioElectronico.Application;

public interface IClienteAppService
{
    Task<ClienteDto> CreateAsync(ClienteCrearDto cliente);
    ListaPaginada<ClienteDto> GetAll(int limit=10,int offset=0);
    Task<ClienteDto> GetByIdAsync(Guid id);

    //Permitir filtrar nombre o por cedula. Paginacion.
    Task<ListaPaginada<ClienteDto>> GetListAsync(ClienteListInput input);

    Task UpdateAsync (Guid id, ClienteActualizarDto cliente);

    Task<bool> DeleteAsync(Guid clienteId);
}

