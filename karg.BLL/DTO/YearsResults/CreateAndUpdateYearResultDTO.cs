namespace karg.BLL.DTO.YearsResults
{
    public class CreateAndUpdateYearResultDTO
    {
        public string Description_en { get; set; }
        public string Description_ua { get; set; }
        public string Year { get; set; }
        public List<string> Images { get; set; }
    }
}