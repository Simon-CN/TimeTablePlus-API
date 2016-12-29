using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimetablePlus_API.Entity
{
    public class DataContext : DbContext
    {
        public DbSet<User> user { get; set; }
        public DbSet<Timeline> timeline { get; set; }
        public DbSet<Lesson> lesson { get; set; }
        public DbSet<LessonMap> lesson_map { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseMySql(@"Server=127.0.0.1;database=timetable_plus;uid=root;pwd=1234;charset=UTF8");

    }
}
