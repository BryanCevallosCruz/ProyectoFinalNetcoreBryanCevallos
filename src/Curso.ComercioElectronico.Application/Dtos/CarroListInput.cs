namespace Curso.ComercioElectronico.Application;

public class CarroListInput {

    public int Limit {get;set;} = 5;
    public int Offset {get;set;} = 0;

    public string? NombreCliente {get;set;}
    
    public Guid? ClienteId{get;set;}

}