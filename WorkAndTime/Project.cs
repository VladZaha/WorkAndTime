using System.ComponentModel.DataAnnotations;

namespace WorkAndTime
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
      
    }
}
