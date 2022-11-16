namespace Curso.ComercioElectronico.Domain;

public interface IBancoClienteRepository :  IRepository<BancoCliente, string> {


    Task<bool> ExisteNombre(string nombre);

    Task<bool> ExisteNombre(string nombre, string idExcluir);


}
