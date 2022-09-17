namespace ApiLibros.Entidades
{
    public class Autor
    {
        public int Id { get; set; }

        public string NombreAutor { get; set; }

        public string ApellidoAutor { get; set; }

        public int LibroId { get; set; }    

        public Libro Libro { get; set; }
    }
}
