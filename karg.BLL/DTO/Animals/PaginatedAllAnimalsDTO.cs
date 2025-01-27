namespace karg.BLL.DTO.Animals
{
    public class PaginatedAllAnimalsDTO
    {
        public List<AnimalDTO> Animals { get; set; }
        public int TotalPages { get; set; }
    }
}