using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadME.Models;
using ReadME.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ReadME.ViewModels;

// ViewModel principal qui affiche la liste des livres
public partial class MainViewModel : ObservableObject
{
    private readonly ApiService _apiService = new();

    // Liste des livres affichés à l'écran
    [ObservableProperty]
    private ObservableCollection<Livre> livres = new();

    // Message d'état (chargement, erreur, etc.)
    [ObservableProperty]
    private string resultMessage = "Chargement des livres...";

    // Commande pour charger les livres depuis l'API
    [RelayCommand]
    public async Task LoadLivresAsync()
    {
        try
        {
            Debug.WriteLine("Chargement des livres...");
            var result = await _apiService.GetLivresAsync();

            if (result.Count == 0)
            {
                Debug.WriteLine("Aucun livre trouvé.");
                ResultMessage = "Aucun livre disponible.";
                return;
            }

            // Affiche dans la console les infos sur les livres reçus
            foreach (var livre in result)
            {
                Debug.WriteLine($"Livre reçu : {livre.Titre} (ID: {livre.Livre_Id})");
                Debug.WriteLine($"Image Base64 Length: {livre.ImageCouverture?.Length}");
                Debug.WriteLine($"ImageSource is null? {(livre.ImageSource == null ? "oui" : "non")}");
            }

            // Met à jour la liste observable
            Livres = new ObservableCollection<Livre>(result);
            ResultMessage = $"Livres reçus : {Livres.Count}";
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erreur lors du chargement : {ex.Message}");
            ResultMessage = $"Erreur : {ex.Message}";
        }
    }

    // Commande appelée lorsqu'on clique sur un livre
    [RelayCommand]
    public async Task SelectLivreAsync(Livre selected)
    {
        if (selected is null)
            return;

        Debug.WriteLine($"Livre sélectionné : {selected.Titre} (ID: {selected.Livre_Id})");
        await Shell.Current.GoToAsync($"DetailsPage?id={selected.Livre_Id}"); // Navigation vers la page de détails
    }
}
