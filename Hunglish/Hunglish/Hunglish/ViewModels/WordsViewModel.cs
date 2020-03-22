using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;
using Hunglish.Database;
using Hunglish.Views;
using Hunglish.Models;

namespace Hunglish.ViewModels
{
    public class WordsViewModel : BaseViewModel
    {
        public ObservableCollection<WordViewModel> Words { get; set; }

        int _lessonId;
        public int LessonId
        {
            get { return _lessonId; }
            set
            {
                SetProperty(ref _lessonId, value);
            }
        }

        int _currentIndex;
        public int CurrentIndex
        {
            get { return _currentIndex; }
            set
            {
                SetProperty(ref _currentIndex, value);
            }
        }

        WordViewModel _currentWord;
        public WordViewModel CurrentWord
        {
            get { return _currentWord; }
            set
            {
                SetProperty(ref _currentWord, value);
            }
        }

        public Command LoadItemsCommand { get; set; }

        public Command ShowAnswerCommand { get; set; }

        public Command<int> DeleteWordCommand { get; set; }

        internal void MoveToNextWord()
        {
            CurrentIndex++;

            if (CurrentIndex > Words.Count - 1)
            {
                CurrentIndex = 0;
            }

            CurrentWord = Words[CurrentIndex];
        }

        public WordsViewModel(int lessonId)
        {
            Words = new ObservableCollection<WordViewModel>();
            LessonId = lessonId;

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            DeleteWordCommand = new Command<int>(async (int id) => await ExecuteDeleteCommand(id));

            ExecuteLoadItemsCommand().ConfigureAwait(false);
        }

        private async Task ExecuteDeleteCommand(int id)
        {
            await State.Database.DeleteWordAsync(id);

            await ExecuteLoadItemsCommand();
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Words.Clear();

                var words = await State.Database.GetWordsAsync(LessonId);
                foreach (var word in words)
                {
                    Words.Add(new WordViewModel(word));
                }

                if (Words?.Count > 0)
                {
                    CurrentWord = Words[CurrentIndex];
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}