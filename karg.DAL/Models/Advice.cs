namespace karg.DAL.Models
{
    public class Advice
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateOnly Created_At { get; set; }
        public int? ImageId { get; set; }
        public Image? Image { get; set; }
    }
}
