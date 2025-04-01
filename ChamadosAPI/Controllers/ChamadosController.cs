using System.Text;
using ChamadosAPI.Data;
using ChamadosAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChamadosAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChamadosController : ControllerBase
{
    private readonly ChamadosDbContext _chamadosDb;

    public ChamadosController(ChamadosDbContext chamadosDb)
    {
        _chamadosDb = chamadosDb;
    }

    [HttpPost("abrir")]
    public async Task<IActionResult> Abrir(ChamadoModel chamado)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        chamado.Data = DateTime.UtcNow;
        chamado.Estado = EstadoChamado.Aberto;
        
        string descricaoFormatada = $"Chamado {chamado.Estado} [{DateTime.UtcNow:dd/MM/yyyy HH:mm}]\n" +
                                    "*** Descrição ***\n" +
                                    chamado.Descricao;
        
        
        chamado.Descricao = descricaoFormatada;
        
        _chamadosDb.Chamados.Add(chamado);
        await _chamadosDb.SaveChangesAsync();
        return RedirectToAction("Listar");
    }

    [HttpPatch("{id}/reabrir")]
    public async Task<IActionResult> Reabrir(int id, [FromBody] AlterarChamadoModel reabertura)
    {
        var chamado = await _chamadosDb.Chamados.FindAsync(id);

        if (chamado == null)
        {
            return NotFound();
        }
        
        if (!Enum.IsDefined(typeof(EstadoChamado), reabertura.Estado))
        {
            return BadRequest("Estado inválido. Utilize apenas 'Aberto', 'Reaberto' ou 'Encerrado'");
        }
        
        chamado.Estado = reabertura.Estado;
        
        var sb = new StringBuilder(chamado.Descricao);
        sb.AppendLine()
            .AppendLine($"{reabertura.Estado} em {DateTime.UtcNow:dd/MM/yyyy HH:mm}")
            .AppendLine("*** Atualização ***")
            .AppendLine(reabertura.Descricao);

        chamado.Descricao = sb.ToString();


        await _chamadosDb.SaveChangesAsync();
        return Ok(chamado);
    }

    [HttpPatch("{id}/encerrar")]
    public async Task<IActionResult> Encerrar(int id, [FromBody] AlterarChamadoModel encerramento)
    {
        var chamado = await _chamadosDb.Chamados.FindAsync(id);

        if (chamado == null)
        {
            return NotFound();
        }
        
        if (!Enum.IsDefined(typeof(EstadoChamado), encerramento.Estado))
        {
            return BadRequest("Estado inválido. Utilize apenas 'Aberto', 'Reaberto' ou 'Encerrado'");
        }

        chamado.Estado = encerramento.Estado;
        
        var sb = new StringBuilder(chamado.Descricao);
        sb.AppendLine()
            .AppendLine($"{encerramento.Estado} em {DateTime.UtcNow:dd/MM/yyyy HH:mm}")
            .AppendLine("*** Motivo ***")
            .AppendLine(encerramento.Descricao);

        chamado.Descricao = sb.ToString();

        await _chamadosDb.SaveChangesAsync();
        return Ok(chamado);
    }
}