namespace Curso.ComercioElectronico.Domain;

public interface ICarroRepository :  IRepository<Carro, Guid> {

    // Los metodos que se puede retornar son los de esta interface mas los de la interfaz que hereda
   //Task<bool> ExisteNombre(string nombre);

    //Task<bool> ExisteNombre(string nombre, Guid idExcluir);

    Task<ICollection<Carro>> GetListAsync(IList<Guid> listaIds, bool asNoTracking = true);
    
    // Task<List<Producto>> ListaProductosAsync (List<Guid> idCarro);
}

public interface ICarroItemRepository :  IRepository<CarroItem, Guid> {

    // Los metodos que se puede retornar son los de esta interface mas los de la interfaz que hereda

    Task<ICollection<CarroItem>> GetListAsync(IList<Guid> listaIds, bool asNoTracking = true);
    
    // Task<List<Producto>> ListaProductosAsync (List<Guid> idCarroItem);
}
