namespace ApiLibros.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        public string NombreLibro { get; set; }
        public string  Fecha { get; set; }

        public List<Autor> autor { get; set; }
    }
}
