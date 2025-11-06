namespace karg.DAL.Models
{
    public class Partner
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Uri? Uri { get; set; }
        public List<Image>? Images { get; set; }
    }
}
