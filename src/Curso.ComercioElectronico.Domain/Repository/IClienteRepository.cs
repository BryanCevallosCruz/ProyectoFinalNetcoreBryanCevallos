namespace Curso.ComercioElectronico.Domain;

public interface IClienteRepository :  IRepository<Cliente, Guid> {

    // Los metodos que se puede retornar son los de esta interface mas los de la interfaz que hereda
    Task<bool> ExisteNombre(string nombre);

    Task<bool> ExisteNombre(string nombre, Guid idExcluir);

    Task<ICollection<Cliente>> GetListAsync(IList<Guid> listaIds, bool asNoTracking = true);
    
    // Task<List<Producto>> ListaProductosAsync (List<int> idProducto);

}
