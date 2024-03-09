namespace Entities.Dtos
{
    // [Serializable] // kapalı tanımlama yaparsak serializable ifadesine ihtiyaç duyarız. açık tanımlama yaptığımız için gerek duymadık.
    public record BookDto
    {
        public int Id {  get; init; }
        public String Title { get; init; }
        public decimal Price { get; init; }
    }
}
