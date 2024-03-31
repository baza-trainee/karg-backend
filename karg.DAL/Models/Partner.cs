namespace karg.DAL.Models
{
    public class Partner
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Uri? Uri { get; set; }
        public int ImageId { get; set; }
        public Image Image { get; set; }
    }
}
