namespace MauiApp1;

public partial class About : ContentPage
{
	public About()
	{
		InitializeComponent();
    }

    private async void OnHomeButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void OnLireButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Lire());
    }
}