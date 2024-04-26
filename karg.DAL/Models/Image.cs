namespace karg.DAL.Models
{
    public class Image
    {
        public int Id { get; set; }
        public Uri? Uri { get; set; }
        public int? AnimalId { get; set; }
        public Advice? Advice { get; set; }
        public YearResult? YearResult { get; set; }
        public Partner? Partner { get; set; }
        public Rescuer? Rescuer { get; set; }
        public Animal? Animal { get; set; }
    }
}
