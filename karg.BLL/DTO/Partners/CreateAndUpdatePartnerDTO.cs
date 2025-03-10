using System.ComponentModel.DataAnnotations;

namespace karg.BLL.DTO.Partners
{
    public class CreateAndUpdatePartnerDTO
    {
        [StringLength(500, ErrorMessage = "Max length is 500 characters")]
        public string Name { get; set; }

        [StringLength(8000, ErrorMessage = "Max length is 8000 characters")]
        public string Uri { get; set; }

        public List<string> Images { get; set; }
    }
}