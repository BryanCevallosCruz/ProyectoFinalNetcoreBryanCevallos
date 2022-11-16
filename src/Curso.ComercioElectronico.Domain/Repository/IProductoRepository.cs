namespace Curso.ComercioElectronico.Domain;

public interface IProductoRepository :  IRepository<Producto, Guid> {

    // Los metodos que se puede retornar son los de esta interface mas los de la interfaz que hereda
    Task<bool> ExisteNombre(string nombre);

    Task<bool> ExisteNombre(string nombre, Guid idExcluir);

    Task<ICollection<Producto>> GetListAsync(IList<Guid> listaIds, bool asNoTracking = true);
    
    // Task<List<Producto>> ListaProductosAsync (List<int> idProducto);

}
