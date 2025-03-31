namespace ChamadosAPI.Models;

public class ChamadoModel
{
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string Usuario { get; set; } = string.Empty;
    public string Departamento { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
}