using MauiApp1.ViewModels;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
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