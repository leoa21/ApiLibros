using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiLibros.Filtros
{
    public class FiltroDeAccion : IActionFilter
    {
        private readonly ILogger<FiltroDeAccion> log;

        public FiltroDeAccion(ILogger<FiltroDeAccion> log)
        {
            this.log = log;
        }

        public void OnActionExecuting(ActionExecutingContext context) //Se procesa antes de que se ejecute la accion
        {
            log.LogInformation("Antes de ejecutar la accion");
        }

        public void OnActionExecuted(ActionExecutedContext context) //Después de que se ejecute
        {
            log.LogInformation("Despues de ejecutar la accion");
        }


    }
}
