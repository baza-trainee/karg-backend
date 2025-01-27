namespace karg.BLL.DTO.Advices
{
    public class PaginatedAllAdvicesDTO
    {
        public List<AdviceDTO> Advices { get; set; }
        public int TotalPages { get; set; }
    }
}