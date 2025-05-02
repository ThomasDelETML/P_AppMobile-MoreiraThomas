namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        public static string BaseAddress =
        DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:3000" : "https://localhost:3000";
        public static string livreURL = $"{BaseAddress}/livres/";

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
