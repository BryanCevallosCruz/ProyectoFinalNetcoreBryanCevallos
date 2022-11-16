namespace Curso.ComercioElectronico.Application;



public interface IOrdenAppService
{
    Task<OrdenDto> GetByIdAsync(Guid id);
    Task<ListaPaginada<OrdenDto>> GetAll(OrdenListInput input);

    Task<OrdenDto> CreateAsync(Guid carroId, OrdenCrearDto ordenDto);

    Task UpdateAsync(Guid id, OrdenActualizarDto orden);

    Task<bool> DeleteAsync(Guid ordenId);

    // IOrdenItemAppService

    Task<OrdenItemDto> GetByIdAsyncItem(Guid id);
    ListaPaginada<OrdenItemDto> GetAllItem(int limit = 10, int offset = 0);

    Task<OrdenItemDto> CreateAsyncItem(Guid id, OrdenItemCrearDto ordenItemDto);

    Task UpdateAsyncItem(Guid id, OrdenItemActualizarDto ordenItemDto);

    Task<bool> DeleteAsyncItem(Guid ordenItemId);
}
