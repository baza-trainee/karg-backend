namespace karg.BLL.DTO.Partners
{
    public class PartnerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Uri Uri { get; set; }
        public List<Uri> Images { get; set; }
    }
}