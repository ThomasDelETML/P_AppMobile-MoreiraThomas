using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Models;
using MauiApp1.Services;
using System.Collections.ObjectModel;

namespace MauiApp1.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ApiService _apiService = new();

    [ObservableProperty]
    private ObservableCollection<Livre> livres = new();

    [ObservableProperty]
    private string resultMessage = "Chargement des livres...";

    [RelayCommand]
    public async Task LoadLivresAsync()
    {
        try
        {
            var result = await _apiService.GetLivresAsync();
            Livres = new ObservableCollection<Livre>(result);
            ResultMessage = $"Livres reçus : {Livres.Count}";
        }
        catch (Exception ex)
        {
            ResultMessage = $"Erreur : {ex.Message}";
        }
    }

    [RelayCommand]
    public async Task SelectLivreAsync(Livre selected)
    {
        if (selected is null)
            return;

        await Shell.Current.GoToAsync($"DetailsPage?id={selected.Livre_Id}");
    }
}
