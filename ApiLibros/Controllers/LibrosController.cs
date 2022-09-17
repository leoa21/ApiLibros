using ApiLibros.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiLibros.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public LibrosController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }


        [HttpGet]
        public async Task<ActionResult<List<Libro>>> Get()
        {
            return await dbContext.Libros.Include(x => x.autor).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Get(Libro libro)
        {
            dbContext.Add(libro);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]

        public async Task<ActionResult> Put(Libro libro, int id)
        {
            if (libro.Id != id)
            {
                return BadRequest("El id no coincide con el establecido en la url");
            }

            dbContext.Update(libro);
            await dbContext.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete("id{id:int}")]

        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Libros.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }

            dbContext.Remove(new Libro()
            {
                Id = id
            });

            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
