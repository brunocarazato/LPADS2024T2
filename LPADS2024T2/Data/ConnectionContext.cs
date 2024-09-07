namespace LPADS2024T2.Data;
using LPADS2024T2.Models;
using Microsoft.EntityFrameworkCore;

    public class ConnectionContext : DbContext
    {

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Curso> Cursos { get; set; }

        public ConnectionContext(DbContextOptions<ConnectionContext> options) : 
        base(options) { }


}
