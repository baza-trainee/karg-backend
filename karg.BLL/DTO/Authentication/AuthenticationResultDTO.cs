namespace karg.BLL.DTO.Authentication
{
    public class AuthenticationResultDTO
    {
        public string? Token { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public int RescuerId { get; set; }
        public bool IsDirector { get; set; }

    }
}
