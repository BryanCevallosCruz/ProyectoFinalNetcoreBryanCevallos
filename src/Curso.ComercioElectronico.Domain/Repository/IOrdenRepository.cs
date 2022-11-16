namespace Curso.ComercioElectronico.Domain;

public interface IOrdenRepository :  IRepository<Orden, Guid> {

    // Los metodos que se puede retornar son los de esta interface mas los de la interfaz que hereda
   //Task<bool> ExisteNombre(string nombre);

    //Task<bool> ExisteNombre(string nombre, Guid idExcluir);

    Task<ICollection<Orden>> GetListAsync(IList<Guid> listaIds, bool asNoTracking = true);
    
    // Task<List<Producto>> ListaProductosAsync (List<Guid> idOrden);
}

public interface IOrdenItemRepository :  IRepository<OrdenItem, Guid> {

    // Los metodos que se puede retornar son los de esta interface mas los de la interfaz que hereda

    Task<ICollection<OrdenItem>> GetListAsync(IList<Guid> listaIds, bool asNoTracking = true);
    
    // Task<List<Producto>> ListaProductosAsync (List<Guid> idOrdenItem);
}
