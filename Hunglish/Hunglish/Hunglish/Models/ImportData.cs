using System.Collections.Generic;

namespace Hunglish.Models
{
    public class ImportData
    {
        public string LessonName { get; set; }

        public List<ImportWord> Words { get; set; }
    }

    public class ImportWord
    {
        public string English { get; set; }

        public string Meaning { get; set; }

        public string ExampleSentence { get; set; }
    }
}
