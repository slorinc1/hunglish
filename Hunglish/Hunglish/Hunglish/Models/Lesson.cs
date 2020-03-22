using System;
using SQLite;

namespace Hunglish.Models
{
    public class Lesson
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Name { get; set; }

        public bool IsCompleted { get; set; }

        public double Score { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
