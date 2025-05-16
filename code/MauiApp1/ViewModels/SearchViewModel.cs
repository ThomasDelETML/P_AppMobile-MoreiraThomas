using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using P_ReadME_GHE.Models;
using P_ReadME_GHE.Services;
using System.Collections.ObjectModel;

namespace P_ReadME_GHE.ViewModels;

public partial class SearchViewModel : ObservableObject
{
    private readonly ApiService _apiService = new();

    [ObservableProperty] private ObservableCollection<string> tags = new();
    [ObservableProperty] private ObservableCollection<Livre> resultats = new();
    [ObservableProperty] private string newTag = "";
    [ObservableProperty] private string titreRecherche = "";
    [ObservableProperty] private bool isFiltresVisible = false;
    [ObservableProperty] private DateTime? dateAjout = null;

    [RelayCommand]
    private void ToggleFiltres()
    {
        IsFiltresVisible = !IsFiltresVisible;
    }

    [RelayCommand]
    private void AddTag()
    {
        if (!string.IsNullOrWhiteSpace(NewTag) && !Tags.Contains(NewTag))
        {
            Tags.Add(NewTag);
            NewTag = string.Empty;
        }
    }

    [RelayCommand]
    private void RemoveTag(string tag)
    {
        if (Tags.Contains(tag))
            Tags.Remove(tag);
    }

    [RelayCommand]
    private async Task SearchAsync()
    {
        var tag = Tags.FirstOrDefault();
        var livres = await _apiService.SearchLivresAsync(TitreRecherche, tag, DateAjout);
        Resultats = new ObservableCollection<Livre>(livres);
    }
}
