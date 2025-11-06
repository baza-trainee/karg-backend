namespace karg.DAL.Models
{
    public class FAQ
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }

        public virtual LocalizationSet Question { get; set; }
        public virtual LocalizationSet Answer { get; set; }
    }
}
