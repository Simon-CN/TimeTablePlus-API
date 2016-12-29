using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TimetablePlus_API.Entity
{
    public class User
    {
        [Column(TypeName = "uint")]
        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        [StringLength(255)]
        public string desc { get; set; }

        [StringLength(255)]
        public string password { get; set; }

        [StringLength(255)]
        public string portrait { get; set; }

        [StringLength(255)]
        public string background { get; set; }

    }
}
