using ApiLibros.Entidades;
using ApiLibros.Filtros;
using ApiLibros.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiLibros.Controllers
{
    [ApiController]
    [Route("/libros")]
    //[Authorize] //A nivel de controlador estamos protegiendo todos los endpoints, si no están autorizados no podrán consumirlos
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IService service;
        private readonly ServiceTransient serviceTransient;
        private readonly ServiceScoped serviceScoped;
        private readonly ServiceSingleton serviceSingleton;
        private readonly ILogger<LibrosController> logger;
        private readonly IWebHostEnvironment env;

        public LibrosController(ApplicationDbContext context, IService service,
            ServiceTransient serviceTransient, ServiceScoped serviceScoped,
            ServiceSingleton serviceSingleton, ILogger<LibrosController> logger,
            IWebHostEnvironment env)
        {
            this.dbContext = context;
            this.service = service;
            this.serviceTransient = serviceTransient;
            this.serviceScoped = serviceScoped;
            this.serviceSingleton = serviceSingleton;
            this.logger = logger;
            this.env = env;
        }

        [HttpGet("GUID")]
      //  [ResponseCache(Duration = 10)] //el tiempo es en segundos //Se utiliza
       // [ServiceFilter(typeof(FiltroDeAccion))]
        public ActionResult ObtenerGuid()
        {
      
            logger.LogInformation("Durante la ejecucion");
            return Ok(new
            {
                LibrosControllerTransient = serviceTransient.guid, //Siempre son diferentes
                ServiceA_Transient = service.GetTransient(),
                LibrosControllerScoped = serviceScoped.guid, //Durante un tiempo es el mismo
                ServiceA_Scoped = service.GetScoped(),
                LibrosControllerSingleton = serviceSingleton.guid, //No cambian
                ServiceA_Singleton = service.GetSingleton()
            });
        }


        [HttpGet("listado")]
        public async Task<ActionResult<List<Libro>>> Get()
        {

            //* Niveles de logs
            // Critical
            // Error
            // Warning
            // Information
            // Debug
            // Trace
            // *//
            //throw new NotImplementedException(); 
            logger.LogInformation("Se obtiene el listado de alumnos");
            logger.LogWarning("Mensaje de prueba warning");
            service.EjecutarJob();
            return await dbContext.Libros.Include(x => x.autor).ToListAsync();
        }
        
        [HttpGet("{nombre}")]
        public async Task<ActionResult<Libro>> Get(string nombre)
        {
            var libro = await dbContext.Libros.FirstOrDefaultAsync(x => x.NombreLibro.Contains(nombre));

            if(libro == null)
            {
                return NotFound();
            }

            return libro;
        }

        [HttpGet("{id:int}")]

        public async Task<ActionResult<Libro>> GetById(int id)
        {
            return await dbContext.Libros.FirstOrDefaultAsync(x => x.Id == id);
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
