namespace Curso.ComercioElectronico.Application;

public interface ICarroAppService
{
    Task<CarroDto> GetByIdAsync(Guid id);
    Task<ListaPaginada<CarroDto>> GetAll(CarroListInput input);

    //Task<ListaPaginada<CarroDto>> GetListAsync(CarroListInput input);

    Task<CarroDto> CreateAsync(CarroCrearDto carro);

    Task UpdateAsync(Guid id, CarroActualizarDto carro);

    Task<bool> DeleteAsync(Guid carroId);

    // ICarroItemAppService

    Task<CarroItemDto> GetByIdAsyncItem(Guid id);
    ListaPaginada<CarroItemDto> GetAllItem(int limit = 10, int offset = 0);

    Task<CarroItemDto> CreateAsyncItem(Guid id, CarroItemCrearDto carroItemDto);

    Task UpdateAsyncItem(Guid id, CarroItemActualizarDto carroItemDto);

    Task<bool> DeleteAsyncItem(Guid carroItemId);

}


