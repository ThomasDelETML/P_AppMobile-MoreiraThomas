using P_ReadME_GHE.ViewModels;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        public static string BaseAddress =
        DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:3000" : "https://localhost:3000";
        public static string livreURL = $"{BaseAddress}/livres/";

        private MainViewModel ViewModel => BindingContext as MainViewModel;

        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
        }
        private async void OnWizardStackTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new About());
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel != null)
                await ViewModel.LoadLivresAsync();
        }
    }

}