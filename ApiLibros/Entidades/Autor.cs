using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiLibros.Entidades
{
    public class Autor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 10, ErrorMessage = "El campom {0} solo puede tener 10 caracteres")]
        public string NombreAutor { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 10, ErrorMessage = "El campom {0} solo puede tener 10 caracteres")]
        public string ApellidoAutor { get; set; }

        [NotMapped]
        [Url]
        public string Url { get; set; }      
        public int LibroId { get; set; }    

        public Libro Libro { get; set; }
    }
}
