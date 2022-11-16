using System.ComponentModel.DataAnnotations;
using Curso.ComercioElectronico.Domain;

namespace Curso.ComercioElectronico.Application;

public class OrdenCrearDto
{

    [Required]
    public DateTime Fecha {get;set;}

    public string? Observaciones { get;set;}
  
} 

public class OrdenActualizarDto
{
    [Required]
    public OrdenEstado Estado {get;set;}
    public string? Observaciones { get;set;}
}  



public class OrdenItemCrearDto {

    [Required]
    public Guid ProductoId {get; set;}
   
    [Required]
    public long Cantidad {get;set;}

    public string? Observaciones { get;set;}
}
public class OrdenItemActualizarDto {

    public string? Observaciones { get;set;}
}



