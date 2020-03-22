using System.Linq;
using Hunglish.Models;
using Hunglish.ViewModels;

namespace Hunglish.Database
{
    public static class State
    {
        static WordDatabase database;

        public static WordDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new WordDatabase();
                }
                return database;
            }
        }

        public static WordViewModel AddNewWord { get; set; }

        public static WordsViewModel CurrentWordsViewModel {get; set;}
        public static LessonsViewModel Lessons { get; internal set; }
        public static int LessonId { get; set; }

        public static bool Flipped { get; set; }

        public static async System.Threading.Tasks.Task AddNewWordCallbackAsync()
        {
            var newWord = AddNewWord as WordViewModel;

            var word = new Word()
            {   ID = newWord.Id,
                LessonId = State.LessonId,
                English = newWord.English,
                Meaning = newWord.Meaning,
                ExampleSentence = newWord.ExampleSentence
            };

            var update = false;
            if (word.ID == 0)
            {
                await Database.InsertWordAsync(word);
            }
            else
            {
                update = true;
                await Database.UpdateWordAsync(word);
            }
            var addedWord = await State.Database.GetWordAsync(LessonId, word.ID);
            var addedWordViewModel = new WordViewModel(addedWord);

            if (update == false)
            {
                CurrentWordsViewModel.Words.Add(addedWordViewModel);
            }
            else
            {
                var updateWord = CurrentWordsViewModel.Words
                    .Where(x => x.Id == addedWordViewModel.Id).SingleOrDefault();

                updateWord.English = addedWordViewModel.English;
                updateWord.Meaning = addedWordViewModel.Meaning;
                updateWord.ExampleSentence = addedWordViewModel.ExampleSentence;
            }
        }
    }
}
