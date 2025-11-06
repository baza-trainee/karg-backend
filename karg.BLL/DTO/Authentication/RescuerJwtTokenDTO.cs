namespace karg.BLL.DTO.Authentication
{
    public class RescuerJwtTokenDTO
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string Role { get; set; }
    }
}