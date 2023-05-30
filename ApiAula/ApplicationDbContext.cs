using ApiAula.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ApiAula
{
    public class ApplicationDbContext : DbContext
    {
        //Add DBSET
        //Ex: public DbSet<ClassType> DbSetName { get; set; }
        public DbSet<Aluno> Alunos { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }







    }
}
