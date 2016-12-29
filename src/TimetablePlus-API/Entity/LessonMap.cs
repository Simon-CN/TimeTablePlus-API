using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TimetablePlus_API.Entity
{
    public class LessonMap
    {
        [Column(TypeName = "uint")]
        [Key]
        public int index { get; set; }
        public int uid { get; set; }
        public int lid { get; set; }

        public LessonMap()
        {

        }
        public LessonMap(int uid, int lid)
        {
            this.uid = uid;
            this.lid = lid;
        }
    }
}
