using ReadME.ViewModels;

namespace ReadME;

public partial class DetailsPage : ContentPage
{
    public DetailsPage()
    {
        InitializeComponent();
        BindingContext = new DetailsViewModel();
    }
}
