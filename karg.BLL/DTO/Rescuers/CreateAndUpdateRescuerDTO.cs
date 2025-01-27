namespace karg.BLL.DTO.Rescuers
{
    public class CreateAndUpdateRescuerDTO
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public List<Uri>? Images { get; set; }
    }
}