namespace Curso.ComercioElectronico.Application;

public class ClienteListInput
{

    public int Limit { get; set; } = 10;

    public int Offset { get; set; } = 0;

    public string? BancoId { get; set; }

    public string? NombreBuscar { get; set; }

    public string? Cedula { get; set; }
}