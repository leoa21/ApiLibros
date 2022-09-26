using ApiLibros.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiLibros.Controllers
{
    [ApiController]
    [Route("/autor")]
    public class AutorController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public AutorController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        [HttpGet("/listado")]

        public async Task<ActionResult<List<Autor>>> GetAll()
        {
            return await dbContext.Autor.ToListAsync();
        }

        [HttpGet("{id:int}")]

        public async Task<ActionResult<Autor>> GetById(int id)
        {
            return await dbContext.Autor.FirstOrDefaultAsync(x => x.Id == id);
        }

        //[HttpGet("{nombre}/{param?})] ---- Es para un parametro opcional

        [HttpGet("{nombre}")]
        public async Task<ActionResult<Autor>> Get(string nombre)
        {
            var autor = await dbContext.Autor.FirstOrDefaultAsync(x => x.NombreAutor.Contains(nombre));

            if (autor == null)
            {
                return NotFound();
            }

            return autor;
        }

        [HttpPost]

        public async Task<ActionResult> Post(Autor autor)
        {
            var existeLibro = await dbContext.Libros.AnyAsync(x => x.Id == autor.LibroId);

            if (!existeLibro)
            {
                return BadRequest($"No existe el libro con el id: { autor.LibroId}");

            }

            dbContext.Add(autor);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]

        public async Task<ActionResult> Put(Autor autor, int id)
        {
            var exist = await dbContext.Autor.AnyAsync(x => x.Id == id);

            if (!exist)
            {
                return NotFound("El autor especificado no existe");
            }

            if (autor.Id != id)
            {
                return BadRequest("El id del autor no coincide con el establecido en la url");

            }

            dbContext.Update(autor);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]

        public async Task<ActionResult> Delete(int id)
        {
           var exist = await dbContext.Autor.AnyAsync(x => x.Id == id);

            if (!exist)
            {
                return NotFound("El Recurso no fue encontrado");
            }

            dbContext.Remove(new Autor { Id = id });
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
