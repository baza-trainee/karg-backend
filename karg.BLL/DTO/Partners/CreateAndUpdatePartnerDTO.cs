namespace karg.BLL.DTO.Partners
{
    public class CreateAndUpdatePartnerDTO
    {
        public string Name { get; set; }
        public Uri Uri { get; set; }
        public List<string> Images { get; set; }
    }
}