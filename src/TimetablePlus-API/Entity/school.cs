using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TimetablePlus_API.Entity
{
    public class School
    {
        [Column(TypeName = "uint")]
        public long id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }
    }
}
