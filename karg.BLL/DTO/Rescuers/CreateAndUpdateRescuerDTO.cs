using System.ComponentModel.DataAnnotations;

namespace karg.BLL.DTO.Rescuers
{
    public class CreateAndUpdateRescuerDTO
    {
        [StringLength(500, ErrorMessage = "Max length is 500 characters")]
        public string? FullName { get; set; }

        [Phone(ErrorMessage = "Неправильний формат номера телефону")]
        public string? PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Неправильний формат електронної пошти")]
        [StringLength(320, ErrorMessage = "Max length is 320 characters")]
        public string? Email { get; set; }

        public List<string>? Images { get; set; }
    }
}