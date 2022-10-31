using ApiLibros;
using ApiLibros.Filtros;
using ApiLibros.Middlewares;
using ApiLibros.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace ApiLibros
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //Principio solid , nuestras clases deberian depender de abstracciones y no tipos concretos
            //var alumnosController = new AlumnosController(new ApplicationDbContext());
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(opciones =>  //Es para que me marque los errores de manera global, cualquier error que suceda en mi aplicacion me los va a marcar
            {
                opciones.Filters.Add(typeof(FiltroDeExcepcion));
            }).AddJsonOptions(x =>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            // Se encarga de configurar ApplicationDbContext como un servicio
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

            //Transient da nueva instancia de la clase declarada, declara una nueva instancia cada vez que se ejecute
            //sirve para funciones que ejecutan una funcionalidad y listo, sin tener
            //que mantener información que será reutilizada en otros lugares
            services.AddTransient<IService, ServiceA>(); //Se hace la inyeccion de manera automatica
            //services.AddTransient<ServiceA>();
            services.AddTransient<ServiceTransient>();
            //Scoped el tiempo de vida de la clase declarada aumenta, sin embargo, Scoped da diferentes instancia
            //de acuerdo a cada quien mande la solicitud es decir Gustavo tiene su intancia y Alumno otra
            //services.AddScoped<IService, ServiceA>();
            services.AddScoped<ServiceScoped>();
            //Singleton se tiene la misma instancia siempre para todos los usuarios en todos los días,
            //todos los usuarios que hagan una petición van a tener la misma info compartida entre todos 
            //services.AddSingleton<IService, ServiceA>();
            services.AddSingleton<ServiceSingleton>();
            services.AddTransient<FiltroDeAccion>();
            services.AddHostedService<EscribirArchivo>();
            services.AddResponseCaching(); //Para el manejo de cache

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiLibros", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            //Use me permite agregar mi propio proceso sin afectar a los demas como Run
            //app.Use(async (context, siguiente) => //Me permite hacer mi middleware personalizado sin afectar los demas
            //{
            //    using (var ms = new MemoryStream())
            //    {
            //        //Se asigna el body del response en una variable y se le da el valor de memorystream
            //        var bodyOriginal = context.Response.Body;
            //        context.Response.Body = ms;

            //        //Permite continuar con la linea
            //        await siguiente.Invoke();

            //        //Guardamos lo que le respondemos al cliente en el string
            //        ms.Seek(0, SeekOrigin.Begin);
            //        string response = new StreamReader(ms).ReadToEnd();
            //        ms.Seek(0, SeekOrigin.Begin);

            //        //Leemos el stream y lo colocamos como estaba
            //        await ms.CopyToAsync(bodyOriginal);
            //        context.Response.Body = bodyOriginal;

            //        logger.LogInformation(response);
            //    }
            //});

            //Metodo para utilizar la clase middleware propia
            //app.UseMiddleware<ResponseHttpMiddleware>();

            //Metodo para utilizar la clase middleware sin exponer la clase. 
            app.UseResponseHttpMiddleware();


            //Atrapara todas las peticiones http que mandemos y retornar un string
            //Para detener todos los otros middleware se utiliza la funcion RUN

            //Para condicionar la ejecucion del middleware segun una ruta especifica se utiliza Map
            //Al utilizar Map permite que en lugar de ejecutar linealmente podemos agregar rutas especificas para
            // nuestro middleware
          app.Map("/maping", app => //El map me permite ramificar los procesos, esto para tener una difercificacion dentro del middleware
           {
                app.Run(async context =>
                {
                   await context.Response.WriteAsync("Interceptando las peticiones"); //Esto solo se va a ejecutar si entramos a la siguiente ruta https://localhost:7155/maping
                });
            });


            // Configure the HTTP request pipeline.
            //El orden del middleware importa
            //Los middleware se definen porque se les antepone un Use
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseResponseCaching(); //Tambien hay que ponerlo en el middleware para poder utilizarlo 

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
