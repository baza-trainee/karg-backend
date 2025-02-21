namespace karg.DAL.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string? Uri { get; set; }
        public int? AdviceId { get; set; }
        public int? AnimalId { get; set; }
        public int? RescuerId { get; set; }
        public int? PartnerId { get; set; }
        public int? YearResultId { get; set; }
        public Advice? Advice { get; set; }
        public YearResult? YearResult { get; set; }
        public Partner? Partner { get; set; }
        public Rescuer? Rescuer { get; set; }
        public Animal? Animal { get; set; }
    }
}
