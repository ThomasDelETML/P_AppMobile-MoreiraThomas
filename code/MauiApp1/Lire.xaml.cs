namespace MauiApp1;

public partial class Lire : ContentPage
{
	public Lire()
	{
		InitializeComponent();
	}

    private async void OnLivreButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new About());
    }

    private async void OnPageButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Lire());
    }
}