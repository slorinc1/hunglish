using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Hunglish.Models;
using Hunglish.Views;
using Hunglish.ViewModels;
using Hunglish.Database;

namespace Hunglish.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        LessonsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            viewModel = new LessonsViewModel();
            State.Lessons = viewModel;

            BindingContext = viewModel;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (LessonsViewModel.SelectedItem != null)
            {
                var lesson = LessonsViewModel.SelectedItem as LessonViewModel;

                // Manually deselect item.
                await Navigation.PushModalAsync(new NavigationPage(new EditWordsList(lesson.Id)));

                State.LessonId = lesson.Id;
                LessonsViewModel.SelectedItem = null;
            }
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Lessons.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var clickedButton = sender as ImageButton;
            var context = clickedButton.BindingContext as LessonViewModel;

            var wordsViewModel = new WordsViewModel(context.Id);
            var learnWordsView = new LearnWordsView
            {
                BindingContext = wordsViewModel
            };

            await Navigation.PushModalAsync(new NavigationPage(learnWordsView));
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            var context = sender as ImageButton;
            var lesson = context.BindingContext as LessonViewModel;

            // Manually deselect item.
            await Navigation.PushModalAsync(new NavigationPage(new EditWordsList(lesson.Id)));

            State.LessonId = lesson.Id;
        }
    }
}