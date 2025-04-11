namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
        }
        private async void OnWizardStackTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new About());
        }

    }

}
