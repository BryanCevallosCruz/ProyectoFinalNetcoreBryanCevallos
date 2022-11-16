using System.ComponentModel.DataAnnotations;
using Curso.ComercioElectronico.Domain;

namespace Curso.ComercioElectronico.Application;


public interface ITipoProductoAppService
{
    Task<TipoProductoDto> CreateAsync(TipoProductoCrearDto tipo);
    
    ICollection<TipoProductoDto> GetAll();

    Task UpdateAsync (string id, TipoProductoActualizarDto tipo);

    Task<bool> DeleteAsync(string tipoId);
}
 