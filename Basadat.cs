//подключаем БД
using Microsoft.EntityFrameworkCore;

namespace KursovoiProekt
{
    class ApplicationContext : DbContext
    {
        public DbSet<Login_parol> LogPar { get; set; } = null!;
        public DbSet<Bond> Bonds { get; set; } = null!;
        public ApplicationContext()
        {
            _ = Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder.UseSqlite("Data Source=Login.db");
        }
    }
    
}


