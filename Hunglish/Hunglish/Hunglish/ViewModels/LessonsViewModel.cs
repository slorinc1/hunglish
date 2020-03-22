using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;
using Hunglish.Database;
using Hunglish.Views;

namespace Hunglish.ViewModels
{
    public class LessonsViewModel : BaseViewModel
    {
        public ObservableCollection<LessonViewModel> Lessons { get; set; }

        public Command LoadItemsCommand { get; set; }

        public LessonsViewModel()
        {
            Lessons = new ObservableCollection<LessonViewModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, LessonViewModel>(this, "AddItem", async (obj, item) =>
            {
                var newLesson = item as LessonViewModel;

                var lessonToAdd = newLesson.ToLesson();
               await State.Database.InsertLessonAsync(lessonToAdd);

                var lessonAdded = await State.Database.GetLessonAsync(lessonToAdd.ID);

                var newLessonViewModel = new LessonViewModel(lessonAdded);

                Lessons.Add(newLessonViewModel);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Lessons.Clear();

                var lessons = await State.Database.GetLessonsAsync();
                foreach (var lesson in lessons)
                {
                    Lessons.Add(new LessonViewModel(lesson));
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