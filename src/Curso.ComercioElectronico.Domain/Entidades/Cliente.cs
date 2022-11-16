using System.ComponentModel.DataAnnotations;


namespace Curso.ComercioElectronico.Domain;

public class Cliente {
    [Required]
    public Guid Id {get;set;}

    [Required]
    [StringLength(DominioConstantes.NOMBRE_MAXIMO)]
    public string Nombre {get;set;}
    [Required]
    public string Cedula {get;set;}
    //buscar un cliente por cedula
    public DateTime? FechaNacimiento {get;set;}
    public string? Direccion {get;set;}
    // correo
    [Required]
    [EmailAddress]
    public string Correo {get;set;}

    [Required]
    public string BancoClienteId {get;set;}
    public virtual BancoCliente BancoCliente {get;set;}

    public decimal PorcentajeDescuento {get;set;}
}


