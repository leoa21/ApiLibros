using ApiLibros.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ApiLibros
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Libro> Libros { get; set; }

        public DbSet<Autor> Autor { get; set; }

    }
}
