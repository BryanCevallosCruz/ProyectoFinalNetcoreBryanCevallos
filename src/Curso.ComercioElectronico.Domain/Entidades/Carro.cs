using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Curso.ComercioElectronico.Domain;

public class Carro
{
    [Required]
    public Guid Id {get;set; }
 
    [Required]
    public Guid ClienteId {get;set;}
   
    public virtual Cliente Cliente {get;set;}

    [Required]
    public DateTime Fecha {get;set;}
    public virtual ICollection<CarroItem> Items {get;set;} = new List<CarroItem>();
    
    [Required]
    public decimal Total {get;set;}

    [Required]
    public CarroEstado Estado {get;set;}

    public void AgregarItem(CarroItem item){
       
        item.Carro = this;
        Items.Add(item); 
    }
}

public class CarroItem {

    [Required]
    public Guid Id {get;set; }

    [Required]
    public Guid ProductoId {get; set;}

    public virtual Producto Producto { get; set; }

    [Required]
    public Guid CarroId {get; set;}

    public virtual Carro Carro { get; set; }

    [Required]
    public long Cantidad {get;set;}

    public decimal Precio {get;set;}
    public string Observaciones { get;set;}
}

public enum CarroEstado{

    Vacio = 0,

    EnProceso = 1,

    Transferido = 2,

}
