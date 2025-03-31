using ChamadosAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ChamadosAPI.Data;

public class ChamadosDbContext : DbContext
{
    public ChamadosDbContext(DbContextOptions<ChamadosDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<ChamadoModel> Chamados { get; set; }
}