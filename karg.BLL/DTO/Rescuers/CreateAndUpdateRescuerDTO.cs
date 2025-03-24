using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace karg.BLL.DTO.Rescuers
{
    public class CreateAndUpdateRescuerDTO
    {
        [StringLength(500, ErrorMessage = "Max length is 500 characters")]
        public string? FullName { get; set; }

        [StringLength(320, ErrorMessage = "Max length is 320 characters")]
        public string? PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Неправильний формат електронної пошти")]
        [StringLength(320, ErrorMessage = "Max length is 320 characters")]
        public string? Email { get; set; }

        [BindNever]
        [JsonIgnore]
        public string? Token { get; set; }

        public List<string>? Images { get; set; }
    }
}