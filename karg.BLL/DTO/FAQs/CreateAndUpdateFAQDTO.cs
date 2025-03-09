using System.ComponentModel.DataAnnotations;

namespace karg.BLL.DTO.FAQs
{
    public class CreateAndUpdateFAQDTO
    {
        [StringLength(500, ErrorMessage = "Max length is 500 characters")]
        public string Question_en { get; set; }

        [StringLength(500, ErrorMessage = "Max length is 500 characters")]
        public string Question_ua { get; set; }

        [StringLength(10000, ErrorMessage = "Max length is 10,000 characters")]
        public string Answer_en { get; set; }

        [StringLength(10000, ErrorMessage = "Max length is 10,000 characters")]
        public string Answer_ua { get; set; }
    }
}