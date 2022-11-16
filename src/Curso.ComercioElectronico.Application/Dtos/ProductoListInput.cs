namespace Curso.ComercioElectronico.Application;

public class ProductoListInput {

    public int Limit {get;set;} = 10;
    public int Offset {get;set;} = 0;

    public string? TipoProductoId {get;set;}
    
    public string? MarcaId {get;set;}

    public string? NombreProducto {get;set;}

}