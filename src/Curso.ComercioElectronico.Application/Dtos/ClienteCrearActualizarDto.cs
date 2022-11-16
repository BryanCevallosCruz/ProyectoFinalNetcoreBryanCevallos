using System.ComponentModel.DataAnnotations;
using Curso.ComercioElectronico.Domain;

namespace Curso.ComercioElectronico.Application;

public class ClienteCrearDto {

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
    public string? BancoClienteId {get;set;}
    //public string BancoCliente {get;set;}
    public decimal PorcentajeDescuento {get;set;}

}

public class ClienteActualizarDto {

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
    public string? BancoClienteId {get;set;}
    public decimal PorcentajeDescuento {get;set;}

}


