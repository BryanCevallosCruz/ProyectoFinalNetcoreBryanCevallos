using System.ComponentModel.DataAnnotations;
using Curso.ComercioElectronico.Domain;

namespace Curso.ComercioElectronico.Application;

public class CarroCrearDto
{
    [Required]
    public Guid ClienteId {get;set;}

    [Required]   
    public virtual ICollection<CarroItemCrearDto> Items {get;set;}

    [Required]
    public DateTime Fecha {get;set;}
  
} 

public class CarroActualizarDto
{
    [Required]
    public CarroEstado Estado {get;set;}
}  


public class CarroItemCrearDto {

    [Required]
    public Guid ProductoId {get; set;}

    [Required]
    public long Cantidad {get;set;}

    public string? Observaciones { get;set;}
}

public class CarroItemActualizarDto {
    public string? Observaciones { get;set;}
}

