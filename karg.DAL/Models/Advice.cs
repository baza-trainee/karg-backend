namespace karg.DAL.Models
{
    public class Advice
    {
        public int Id { get; set; }
        public int TitleId { get; set; }
        public int DescriptionId { get; set; }
        public DateOnly Created_At { get; set; }
        public List<Image>? Images { get; set; }

        public virtual LocalizationSet Title { get; set; }
        public virtual LocalizationSet Description { get; set; }
    }
}
