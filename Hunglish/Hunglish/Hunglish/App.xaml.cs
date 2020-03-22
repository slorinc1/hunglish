using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Hunglish.Services;
using Hunglish.Views;

namespace Hunglish
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
