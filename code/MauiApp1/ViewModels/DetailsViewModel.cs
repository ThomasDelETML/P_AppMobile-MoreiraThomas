using CommunityToolkit.Mvvm.ComponentModel;
using P_ReadME_GHE.Models;
using P_ReadME_GHE.Services;

namespace P_ReadME_GHE.ViewModels;

[QueryProperty(nameof(LivreId), "id")]
public partial class DetailsViewModel : ObservableObject
{
    private readonly ApiService _apiService = new();

    [ObservableProperty]
    private string title = "Titre du livre";

    [ObservableProperty]
    private string resume = "Résumé du livre";

    [ObservableProperty]
    private int livreId;

    partial void OnLivreIdChanged(int value)
    {
        LoadLivre();
    }

    private async void LoadLivre()
    {
        var livre = await _apiService.GetLivreByIdAsync(LivreId);

        if (livre is not null)
        {
            Title = livre.Titre ?? "Titre inconnu";
            Resume = string.IsNullOrWhiteSpace(livre.Resume) ? "Aucun résumé disponible." : livre.Resume;
        }
        else
        {
            Title = "Livre introuvable";
            Resume = "";
        }
    }
}
