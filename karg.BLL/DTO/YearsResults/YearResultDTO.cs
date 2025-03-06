namespace karg.BLL.DTO.YearsResults
{
    public class YearResultDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Created_At { get; set; }
        public List<string> Images { get; set; }
    }
}