namespace Example.App
{
    using Example.App.Views;

    using Xamarin.Forms;

    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.MainPage = new HomeView();
        }
    }
}