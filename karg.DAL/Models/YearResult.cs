namespace karg.DAL.Models
{
    public class YearResult
    {
        public int Id { get; set; }
        public DateOnly Year { get; set; }
        public string? Description { get; set; }
        public int ImageId { get; set; }
        public Image? Image { get; set; }
    }
}
