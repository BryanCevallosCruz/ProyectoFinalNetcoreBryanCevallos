using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Curso.ComercioElectronico.Domain;

public class Orden
{
    [Required]
    public Guid Id {get;set; }
 
    [Required]
    public Guid ClienteId {get;set;}
   
    public virtual Cliente Cliente {get;set;}

    [Required]
    public DateTime Fecha {get;set;}

    public DateTime? FechaAnulacion {get;set;} 

    public virtual ICollection<OrdenItem> Items {get;set;} = new List<OrdenItem>();

    [Required]
    public decimal Total {get;set;}

    public string? Observaciones { get;set;}

    [Required]
    public OrdenEstado Estado {get;set;}

    public void AgregarItem(OrdenItem item){
        item.Orden = this;
        Items.Add(item); 
    }
}

public class OrdenItem {

    [Required]
    public Guid Id {get;set; }

    [Required]
    public Guid ProductoId {get; set;}

    public virtual Producto Producto { get; set; }

    [Required]
    public Guid OrdenId {get; set;}

    public virtual Orden Orden { get; set; }

    [Required]
    public long Cantidad {get;set;}

    public decimal Precio {get;set;}

    public string? Observaciones { get;set;}
}

public enum OrdenEstado{

    Anulada = 0,

    EnProceso = 1,

    Registrada = 2,

    Entregada = 3
}
