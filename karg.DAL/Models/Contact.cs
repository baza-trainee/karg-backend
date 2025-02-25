using karg.DAL.Models.Enums;

namespace karg.DAL.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public ContactCategory Category { get; set; }
        public string Value { get; set; }
    }
}
