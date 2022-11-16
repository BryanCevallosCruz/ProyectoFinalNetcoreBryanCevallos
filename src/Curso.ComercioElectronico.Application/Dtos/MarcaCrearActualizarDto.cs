using System.ComponentModel.DataAnnotations;
using Curso.ComercioElectronico.Domain;

namespace Curso.ComercioElectronico.Application;

public class MarcaCrearDto
{
    [Required]
    [StringLength(DominioConstantes.LONGITUD_MAXIMA_MARCA)]
    public string Id {get;set;}
    [Required]
    [StringLength(DominioConstantes.NOMBRE_MAXIMO)]
    public string Nombre {get;set;}
}
public class MarcaActualizarDto
{
    [Required]
    [StringLength(DominioConstantes.NOMBRE_MAXIMO)]
    public string Nombre {get;set;}
}