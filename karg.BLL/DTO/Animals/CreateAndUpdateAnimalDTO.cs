namespace karg.BLL.DTO.Animals
{
    public class CreateAndUpdateAnimalDTO
    {
        public string Category { get; set; }
        public string Name_en { get; set; }
        public string Description_en { get; set; }
        public string Story_en { get; set; }
        public string Name_ua { get; set; }
        public string Description_ua { get; set; }
        public string Story_ua { get; set; }
        public List<string> Images { get; set; }
    }
}