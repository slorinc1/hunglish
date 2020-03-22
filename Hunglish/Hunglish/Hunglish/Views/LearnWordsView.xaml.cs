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
    public partial class LearnWordsView : ContentPage
    {
        public LearnWordsView()
        {
            InitializeComponent();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var context = sender as Button;

            var vm = context.BindingContext as WordsViewModel;

            vm.CurrentWord.ShowAnswer();

            XFFlipViewControl1.IsFlipped = !XFFlipViewControl1.IsFlipped;
        }

        private async void Right_Clicked(object sender, EventArgs e)
        {
            XFFlipViewControl1.IsFlipped = !XFFlipViewControl1.IsFlipped;

            var context = sender as ImageButton;

            var vm = context.BindingContext as WordsViewModel;
            vm.CurrentWord.AnswerVisible = false;

            await vm.CurrentWord.AnswerAsync(true);

            vm.MoveToNextWord();
        }

        private async void Wrong_Clicked(object sender, EventArgs e)
        {
            XFFlipViewControl1.IsFlipped = !XFFlipViewControl1.IsFlipped;

            var context = sender as ImageButton;

            var vm = context.BindingContext as WordsViewModel;
            vm.CurrentWord.AnswerVisible = false;
            await vm.CurrentWord.AnswerAsync(false);

            vm.MoveToNextWord();
        }
    }
}
