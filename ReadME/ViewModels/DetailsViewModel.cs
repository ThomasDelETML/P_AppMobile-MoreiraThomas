using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadME.Models;
using ReadME.Services;
using Microsoft.Maui.Controls;
using System.IO;
using System.Diagnostics;

namespace ReadME.ViewModels;

// Permet de récupérer l'ID du livre via la navigation (QueryProperty)
[QueryProperty(nameof(LivreId), "id")]
public partial class DetailsViewModel : ObservableObject
{
    private readonly ApiService _apiService = new();

    // Propriétés affichées dans la page
    [ObservableProperty] private string title = "Titre du livre";
    [ObservableProperty] private string resume = "Résumé du livre";
    [ObservableProperty] private string auteur = "Inconnu";
    [ObservableProperty] private string langue = "";
    [ObservableProperty] private string sujet = "";
    [ObservableProperty] private ImageSource? imageSource;
    [ObservableProperty] private int livreId;
    [ObservableProperty] private string dateModification = "";

    // Commande appelée quand on veut lire le livre
    [RelayCommand]
    async Task LireLivreAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(LirePage)}?id={LivreId}");
    }

    // Déclenché automatiquement quand LivreId change
    partial void OnLivreIdChanged(int value)
    {
        Debug.WriteLine($"LivreId changé : {value}");
        LoadLivre(); // On charge les données du livre
    }

    // Commande pour revenir à la page précédente
    [RelayCommand]
    async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    // Méthode qui récupère les infos du livre depuis l'API
    private async void LoadLivre()
    {
        Debug.WriteLine($"Chargement du livre ID: {LivreId}");

        try
        {
            var livre = await _apiService.GetLivreByIdAsync(LivreId);

            if (livre is not null)
            {
                // Remplissage des propriétés avec les données reçues
                Title = livre.Titre ?? "Titre inconnu";
                Auteur = livre.Auteur ?? "Auteur inconnu";
                Langue = livre.Langue ?? "";
                Sujet = livre.Sujet ?? "";
                DateModification = livre.Date_Modification?.ToString("yyyy-MM-dd") ?? "";

                // Création d’un extrait du contenu
                if (!string.IsNullOrWhiteSpace(livre.Contenu))
                {
                    var extrait = livre.Contenu.Length > 400 ? livre.Contenu[..400] + "..." : livre.Contenu;
                    Resume = extrait;
                    Debug.WriteLine("Résumé extrait du contenu");
                }
                else
                {
                    Resume = "Aucun contenu disponible.";
                    Debug.WriteLine("Aucun contenu dans le livre");
                }

                // Chargement de l'image de couverture si disponible
                if (!string.IsNullOrWhiteSpace(livre.ImageCouverture))
                {
                    try
                    {
                        ImageSource = ImageSource.FromStream(() =>
                            new MemoryStream(Convert.FromBase64String(livre.ImageCouverture)));
                        Debug.WriteLine("Image chargée avec succès.");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Erreur lors du chargement de l'image : {ex.Message}");
                        ImageSource = null;
                    }
                }
            }
            else
            {
                Title = "Livre introuvable";
                Resume = "";
                Debug.WriteLine("Livre non trouvé");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erreur chargement livre : {ex.Message}");
            Resume = "Erreur lors du chargement.";
        }
    }
}
