using System.ComponentModel.DataAnnotations;

namespace Curso.ComercioElectronico.Domain;


public class BancoCliente
{
    [Required]
    [StringLength(DominioConstantes.LONGITUD_MAXIMA)]
    public string Id {get;set;}

    [Required]
    [StringLength(DominioConstantes.NOMBRE_MAXIMO)]
    public string Nombre {get;set;}

}




