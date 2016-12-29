using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TimetablePlus_API.Entity
{
    public class Lesson
    {
        [Column(TypeName = "uint")]
        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        [StringLength(255)]
        public string classroom { get; set; }

        [StringLength(255)]
        public string teacher { get; set; }

        public int dayofweek { get; set; }

        public int startTime { get; set; }

        public int endTime { get; set; }

        public int startWeek { get; set; }

        public int endWeek { get; set; }

    }
}
