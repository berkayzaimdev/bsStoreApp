namespace Entities.Dtos
{
    // [Serializable] // kapalı tanımlama yaparsak serializable ifadesine ihtiyaç duyarız. açık tanımlama yaptığımız için gerek duymadık.
    public record BookDto
    {
        public int Id {  get; set; }
        public String Title { get; set; }
        public decimal Price { get; set; }
    }
}
