using System.ComponentModel.DataAnnotations;

namespace karg.BLL.DTO.Animals
{
    public class CreateAndUpdateAnimalDTO
    {
        public string Category { get; set; }

        [StringLength(500, ErrorMessage = "Max length is 500 characters")]
        public string Name_en { get; set; }

        [StringLength(10000, ErrorMessage = "Max length is 10,000 characters")]
        public string Description_en { get; set; }

        [StringLength(10000, ErrorMessage = "Max length is 10,000 characters")]
        public string Story_en { get; set; }

        [StringLength(500, ErrorMessage = "Max length is 500 characters")]
        public string Name_ua { get; set; }

        [StringLength(10000, ErrorMessage = "Max length is 10,000 characters")]
        public string Description_ua { get; set; }

        [StringLength(10000, ErrorMessage = "Max length is 10,000 characters")]
        public string Story_ua { get; set; }

        public List<string> Images { get; set; }
    }
}