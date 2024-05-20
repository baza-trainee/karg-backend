namespace karg.DAL.Models
{
    public class YearResult
    {
        public int Id { get; set; }
        public DateOnly Year { get; set; }
        public int DescriptionId { get; set; }
        public int ImageId { get; set; }
        public Image? Image { get; set; }

        public virtual LocalizationSet Description { get; set; }
    }
}
