using karg.DAL.Models.Enums;

namespace karg.DAL.Models
{
    public class Animal
    {
        public int Id { get; set; }
        public int NameId { get; set; }
        public AnimalCategory Category { get; set; }
        public int DescriptionId { get; set; }
        public int StoryId { get; set; }
        public decimal Donats { get; set; }
        public List<Image>? Images { get; set; }

        public virtual LocalizationSet Name { get; set; }
        public virtual LocalizationSet Description { get; set; }
        public virtual LocalizationSet Story { get; set; }
    }
}
