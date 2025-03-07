namespace karg.BLL.DTO.YearsResults
{
    public class CreateAndUpdateYearResultDTO
    {
        public string Title_en { get; set; }
        public string Title_ua { get; set; }
        public string Description_en { get; set; }
        public string Description_ua { get; set; }
        public string Created_At { get; set; }
        public List<string> Images { get; set; }
    }
}