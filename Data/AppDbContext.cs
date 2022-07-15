using Microsoft.EntityFrameworkCore;
using MinimalAPI.Models;

namespace MinimalAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options ): base( options )
        {}

        public DbSet<Tarefa> Tarefas => Set<Tarefa>();
    }
}
