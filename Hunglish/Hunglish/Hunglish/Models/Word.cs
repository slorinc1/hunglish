using SQLite;

namespace Hunglish.Models
{
    public class Word
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int LessonId { get; set; }
        public string English { get; set; }
        public string Meaning { get; set; }
        public string ExampleSentence { get; set; }
        public int IKnowCount { get; set; }
        public int IDontKnowCount { get; set; }
        public bool LatestAnswer { get; set; }
    }
}
