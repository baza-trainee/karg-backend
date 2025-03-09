using System.ComponentModel.DataAnnotations;

namespace karg.BLL.DTO.Advices
{
    public class CreateAndUpdateAdviceDTO
    {
        [StringLength(500, ErrorMessage = "Max length is 500 characters")]
        public string Title_en { get; set; }

        [StringLength(500, ErrorMessage = "Max length is 500 characters")]
        public string Title_ua { get; set; }

        [StringLength(10000, ErrorMessage = "Max length is 10,000 characters")]
        public string Description_en { get; set; }

        [StringLength(10000, ErrorMessage = "Max length is 10,000 characters")]
        public string Description_ua { get; set; }

        public string Created_At { get; set; }

        public List<string> Images { get; set; }
    }
}