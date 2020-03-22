using System;
using System.Linq;
using System.Threading.Tasks;
using Hunglish.Database;
using Hunglish.Models;

namespace Hunglish.ViewModels
{
    public class WordViewModel : BaseViewModel
    {
        public WordViewModel() { }

        public WordViewModel(Word word)
        {
            if (word != null)
            {
                Id = word.ID;
                LessonId = word.LessonId;
                English = word.English;
                Meaning = word.Meaning;
                ExampleSentence = word.ExampleSentence;

                if (word.IKnowCount == 0 && word.IDontKnowCount == 0)
                {
                    Status = "New";
                }
                else
                {
                    if (word.IKnowCount > 0 || word.IDontKnowCount > 0)
                    {
                        Status = "Learning";
                    }

                    if (word.IKnowCount > 2 && word.LatestAnswer)
                    {
                        Status = "Mastered";
                    }
                }
            }
        }

        public void ShowAnswer()
        {
            AnswerVisible = true;
        }

        int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                SetProperty(ref _id, value);
            }
        }

        int _lessonId;
        public int LessonId
        {
            get { return _lessonId; }
            set
            {
                SetProperty(ref _lessonId, value);
            }
        }

        internal async Task AnswerAsync(bool right)
        {
            var word = await State.Database.GetWordAsync(LessonId, Id);

            if (right)
            {
                word.IKnowCount += 1;
            }
            else
            {
                word.IDontKnowCount += 1;
            }
            word.LatestAnswer = right;

            var res = await State.Database.UpdateWordAsync(word);
            var newWord = await State.Database.GetWordAsync(LessonId, Id);
            var newViewModel = new WordViewModel(newWord);

            this.Status = newViewModel.Status;

            if (right)
            {
                var lesson = await State.Database.GetLessonAsync(newViewModel.LessonId);
                var wordsCount = await State.Database.GetWordsCountAsync(newViewModel.LessonId);
                lesson.Score += 1 / (double)wordsCount;
                var newLessonViewModel = new LessonViewModel(lesson);

                var oldLesson = State.Lessons.Lessons.Where(x => x.Id == lesson.ID).SingleOrDefault();
                oldLesson.Score = newLessonViewModel.Score;

                lesson.Score = oldLesson.Score;
                await State.Database.UpdateLessonAsync(lesson);
            }
        }

        bool _answerVisible;
        public bool AnswerVisible
        {
            get { return _answerVisible; }
            set
            {
                SetProperty(ref _answerVisible, value);
            }
        }

        string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                SetProperty(ref _status, value);
            }
        }

        string _english;
        public string English
        {
            get { return _english; }
            set
            {
                SetProperty(ref _english, value);
            }
        }

        string _meaning;
        public string Meaning
        {
            get { return _meaning; }
            set
            {
                SetProperty(ref _meaning, value);
            }
        }

        string _exampleSentence;
        public string ExampleSentence
        {
            get { return _exampleSentence; }
            set
            {
                SetProperty(ref _exampleSentence, value);
            }
        }
    }
}