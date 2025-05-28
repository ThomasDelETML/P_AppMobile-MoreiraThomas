using ReadME.Models;
using ReadME.ViewModels;
using System.Diagnostics;

namespace ReadME;

// Code-behind de la page principale qui affiche la liste des livres
public partial class MainPage : ContentPage
{
    // Accès au ViewModel lié à cette page
    private MainViewModel ViewModel => BindingContext as MainViewModel;

    public MainPage()
    {
        InitializeComponent(); // Initialise la vue (définie dans MainPage.xaml)
        BindingContext = new MainViewModel(); // Lien entre la vue et le ViewModel
    }

    // Méthode appelée automatiquement quand la page devient visible
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (ViewModel != null)
        {
            Debug.WriteLine("MainPage OnAppearing: chargement des livres");
            await ViewModel.LoadLivresAsync(); // Charge les livres à l'ouverture
        }
    }

    // Méthode appelée quand l'utilisateur clique sur un livre dans la liste
    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel == null || e.CurrentSelection.FirstOrDefault() is not Livre selected)
            return;

        Debug.WriteLine($"Livre cliqué : {selected.Titre} (ID: {selected.Livre_Id})");
        await ViewModel.SelectLivreAsync(selected); // Ouvre la page de détails

        BooksCollection.SelectedItem = null; // Réinitialise la sélection
    }
}
