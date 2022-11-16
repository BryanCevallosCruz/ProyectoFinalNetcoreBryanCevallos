using System.ComponentModel.DataAnnotations;
using Curso.ComercioElectronico.Domain;

namespace Curso.ComercioElectronico.Application;


public interface IMarcaAppService
{

    ICollection<MarcaDto> GetAll();

    Task<MarcaDto> CreateAsync(MarcaCrearDto marca);

    Task UpdateAsync (string id, MarcaActualizarDto marca);

    Task<bool> DeleteAsync(string marcaId);
}
 
 
