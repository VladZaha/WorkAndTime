using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkAndTime
{
    public class History
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string TimePeriod { get; set; }
        public int ProjectId { get; set; }
        [NotMapped]
        public string Progress { get; set; }
        public virtual Project Project { get; set; }
    }
}
