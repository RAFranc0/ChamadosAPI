namespace ChamadosAPI.Models;

public class AlterarChamadoModel
{
    public EstadoChamado Estado { get; set; }
    public string Descricao { get; set; } = string.Empty;
}