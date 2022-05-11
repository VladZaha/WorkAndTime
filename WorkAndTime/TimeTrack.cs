using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkAndTime
{
    public class TimeTrack
    {
        [Key]
        public int id { get; set; }
        public string startTime { get; set; }
        public string stopTime { get; set; }
        public int historyId { get; set; }
        public History History { get; set; }
    }
}
