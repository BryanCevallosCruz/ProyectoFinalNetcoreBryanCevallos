using System.ComponentModel.DataAnnotations;
using Curso.ComercioElectronico.Domain;

namespace Curso.ComercioElectronico.Application;

public class BancoClienteDto
{
    [Required]
    [StringLength(DominioConstantes.LONGITUD_MAXIMA)]
    public string Id {get;set;}

    [Required]
    [StringLength(DominioConstantes.NOMBRE_MAXIMO)]
    public string Nombre {get;set;}

}




