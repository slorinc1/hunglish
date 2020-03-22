using System.Threading.Tasks;
using Hunglish.Database;
using Hunglish.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Hunglish.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewWordPage : ContentPage
    {
        public WordViewModel Word { get; set; }

        public NewWordPage(int lessonId, WordViewModel vm = null)
        {
            InitializeComponent();

            if (vm == null)
            {
                Word = new WordViewModel()
                {
                    LessonId = lessonId
                };
            }
            else
            {
                Word = vm;
            }

            State.AddNewWord = Word;
            BindingContext = this;
        }

        async void Cancel_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Save_Clicked(object sender, System.EventArgs e)
        {
            //MessagingCenter.Send(this, "AddNewWord", Word);

            await State.AddNewWordCallbackAsync();

            await Navigation.PopModalAsync();
        }
    }
}