using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadME.Models;
using ReadME.Services;
using System.Diagnostics;

namespace ReadME.ViewModels;

// Permet de récupérer l'ID du livre à afficher grâce à la navigation
[QueryProperty(nameof(LivreId), "id")]
public partial class LireViewModel : ObservableObject
{
    private readonly ApiService _apiService = new();

    // Propriétés liées à l'affichage du livre
    [ObservableProperty] private string title = "";
    [ObservableProperty] private string auteur = "";
    [ObservableProperty] private string contenu = "Chargement...";
    [ObservableProperty] private int livreId;

    // Appelée automatiquement quand l'ID change
    partial void OnLivreIdChanged(int value)
    {
        Debug.WriteLine($"Lecture du livre ID: {value}");
        LoadLivre(); // Récupère les infos du livre
    }

    // Commande pour revenir à la page précédente
    [RelayCommand]
    async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    // Charge les données du livre via l'API
    private async void LoadLivre()
    {
        try
        {
            var livre = await _apiService.GetLivreByIdAsync(LivreId);

            if (livre != null)
            {
                Title = livre.Titre ?? "Titre inconnu";
                Auteur = livre.Auteur ?? "Auteur inconnu";
                Contenu = string.IsNullOrWhiteSpace(livre.Contenu)
                    ? "Ce livre ne contient pas encore de texte."
                    : livre.Contenu;
            }
            else
            {
                Contenu = "Erreur : livre introuvable.";
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erreur chargement lecture : {ex.Message}");
            Contenu = "Erreur lors du chargement du contenu.";
        }
    }
}
