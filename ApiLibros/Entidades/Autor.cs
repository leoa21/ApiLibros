using ApiLibros.Validaciones;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiLibros.Entidades
{
    public class Autor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 10, ErrorMessage = "El campom {0} solo puede tener 10 caracteres")]
        [PrimeraLetraMayuscula]
        public string NombreAutor { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 10, ErrorMessage = "El campom {0} solo puede tener 10 caracteres")]

        public string ApellidoAutor { get; set; }

        [NotMapped]
        [Url]
        public string Url { get; set; }      
        public int LibroId { get; set; }    
        public Libro Libro { get; set; }


        //Si la validacion se va a utilizar solamente para una sola entidad se puede hacer lo siguiete
        //Pero es más recomendable crear una clase personalizada para nada más importar la validacion en cada entidad. 
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            //Para que se ejecuten estas restricciones primero se tienen que cumplir las reglas por atributo, por ejemplo: Range
            //Primero se ejecutaran las validaciones mappeadas en los atributos y después las declaradas en la entidad
            if (!string.IsNullOrEmpty(NombreAutor))
            {
                var primeraLetra = NombreAutor[0].ToString(); //

                if(primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser mayuscula", //Con la sentencia yield agregamos los valores a la lista IEnumerable
                        new String[] {nameof(NombreAutor) }); //El error se va a definir de acuardo a lo que pongamos aqui, en nameof
                }

            }
        }


    }
}
