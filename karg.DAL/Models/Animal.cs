using karg.DAL.Models.Enums;

namespace karg.DAL.Models
{
    public class Animal
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public AnimalCategory Сategory { get; set; }
        public string? Short_Description { get; set; }
        public DateOnly Date_Of_Birth { get; set; }
        public string? Description { get; set; }
        public string? Story { get; set; }
        public decimal Donats { get; set; }
        public List<Image>? Images { get; set; }
    }
}
