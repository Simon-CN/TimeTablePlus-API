using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TimetablePlus_API.Entity
{
    public class Timeline
    {
        [Column(TypeName = "uint")]
        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string content { get; set; }

        public DateTime createTime { get; set; }

        public int lessonId { get; set; }

        public int userId { get; set; }

        [StringLength(3000)]
        public string pictures { get; set; }

        [StringLength(255)]
        public string location { get; set; }
    }
}
