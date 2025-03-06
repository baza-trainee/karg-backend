namespace karg.DAL.Models
{
    public class YearResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int DescriptionId { get; set; }
        public DateOnly Created_At { get; set; }
        public List<Image>? Images { get; set; }

        public virtual LocalizationSet Description { get; set; }
    }
}
