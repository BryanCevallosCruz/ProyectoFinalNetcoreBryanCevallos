using System.ComponentModel.DataAnnotations;
using Curso.ComercioElectronico.Domain;

namespace Curso.ComercioElectronico.Application;


public interface IBancoClienteAppService
{
    Task<BancoClienteDto> CreateAsync(BancoClienteCrearDto banco);
    ICollection<BancoClienteDto> GetAll();

    Task UpdateAsync (string id, BancoClienteActualizarDto banco);

    Task<bool> DeleteAsync(string bancoId);
}
 