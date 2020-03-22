using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Hunglish.Database;
using Hunglish.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Hunglish.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditWordsList : ContentPage
    {
        public WordsViewModel Words { get; set; }

        public EditWordsList(int lessonId)
        {
            InitializeComponent();

            Words = new WordsViewModel(lessonId);

            State.CurrentWordsViewModel = Words;

            BindingContext = Words;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            var button = sender as ImageButton;
            var vm = button?.BindingContext as WordViewModel;

            var newVm = new WordViewModel() 
            {
                English = vm?.English,
                Meaning = vm?.Meaning,
                ExampleSentence = vm?.ExampleSentence,
                Id = vm?.Id ?? 0,
                LessonId = vm?.LessonId ?? 0,
                Status = vm?.Status,
                AnswerVisible = vm?.AnswerVisible ?? false
            };
            var newWordPage = new NewWordPage(Words.LessonId, newVm);
            
            await Navigation.PushModalAsync(new NavigationPage(newWordPage));
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
