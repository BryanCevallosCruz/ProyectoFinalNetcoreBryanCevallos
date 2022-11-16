using System.ComponentModel.DataAnnotations;
using Curso.ComercioElectronico.Domain;

namespace Curso.ComercioElectronico.Application;

public class CarroDto
{
    [Required]
    public Guid Id {get;set; }
 
    [Required]
    public Guid ClienteId {get;set;}
   
    public string? Cliente {get;set;}

    [Required]
    public DateTime Fecha {get;set;}

    public virtual ICollection<CarroItemDto> Items {get;set;}

    [Required]
    public decimal Total {get;set;}

    [Required]
    public CarroEstado Estado {get;set;}
 
}


public class CarroItemDto {

    [Required]
    public Guid Id {get;set; }

    [Required]
    public Guid ProductoId {get; set;}

    public string? Producto { get; set; }

    [Required]
    public Guid CarroId {get; set;}
   
    [Required]
    public long Cantidad {get;set;}
    public decimal Precio {get;set;}
    public string? Observaciones { get;set;}
}
