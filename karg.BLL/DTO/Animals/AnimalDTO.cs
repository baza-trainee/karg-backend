namespace karg.BLL.DTO.Animals
{
    public class AnimalDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Story { get; set; }
        public string Image { get; set; }
        public List<string> Images { get; set; }
    }
}