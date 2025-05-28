using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadME.Models;
using ReadME.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ReadME.ViewModels;

// ViewModel pour la page de recherche de livres
public partial class SearchViewModel : ObservableObject
{
    private readonly ApiService _apiService = new();

    // Liste des tags choisis pour la recherche
    [ObservableProperty] private ObservableCollection<string> tags = new();

    // Résultats de la recherche
    [ObservableProperty] private ObservableCollection<Livre> resultats = new();

    // Champ pour saisir un nouveau tag
    [ObservableProperty] private string newTag = "";

    // Champ de recherche par titre
    [ObservableProperty] private string titreRecherche = "";

    // Affichage ou non des filtres
    [ObservableProperty] private bool isFiltresVisible = false;

    // Filtre par date d’ajout/modification
    [ObservableProperty] private DateTime? dateAjout = null;

    // Affiche ou cache les filtres
    [RelayCommand]
    private void ToggleFiltres()
    {
        IsFiltresVisible = !IsFiltresVisible;
        Debug.WriteLine($"Filtres visibles : {IsFiltresVisible}");
    }

    // Ajoute un tag si ce n’est pas un doublon
    [RelayCommand]
    private void AddTag()
    {
        if (!string.IsNullOrWhiteSpace(NewTag) && !Tags.Contains(NewTag))
        {
            Tags.Add(NewTag);
            Debug.WriteLine($"Tag ajouté : {NewTag}");
            NewTag = string.Empty;
        }
    }

    // Supprime un tag de la liste
    [RelayCommand]
    private void RemoveTag(string tag)
    {
        if (Tags.Contains(tag))
        {
            Tags.Remove(tag);
            Debug.WriteLine($"Tag supprimé : {tag}");
        }
    }

    // Lance la recherche avec les filtres appliqués
    [RelayCommand]
    private async Task SearchAsync()
    {
        try
        {
            string query = TitreRecherche;

            // Si des tags sont présents, on les ajoute à la recherche
            if (Tags.Any())
            {
                var tagsConcat = string.Join(" ", Tags);
                query = string.IsNullOrWhiteSpace(query) ? tagsConcat : $"{query} {tagsConcat}";
            }

            var livres = new List<Livre>();

            // Si aucun filtre n’est défini, on affiche un message
            if (string.IsNullOrWhiteSpace(query) && !DateAjout.HasValue)
            {
                await Shell.Current.DisplayAlert("Recherche", "Veuillez entrer un titre, tag ou une date.", "OK");
                return;
            }

            // Si une recherche textuelle est définie
            if (!string.IsNullOrWhiteSpace(query))
            {
                Debug.WriteLine($"Recherche avec query = \"{query}\"");
                livres = await _apiService.SearchLivresAsync(query);
            }
            else
            {
                // Si seule une date est définie
                Debug.WriteLine("Recherche uniquement par date, récupération de tous les livres...");
                livres = await _apiService.GetLivresAsync();
            }

            // Filtrage par date si applicable
            if (DateAjout.HasValue)
            {
                var targetDate = DateAjout.Value.Date;
                livres = livres.Where(l =>
                    l.Date_Modification?.Date == targetDate ||
                    l.CreatedAt.Date == targetDate
                ).ToList();

                Debug.WriteLine($"Résultats pour le {targetDate:yyyy-MM-dd} : {livres.Count} livre(s)");
            }

            if (livres.Count == 0)
            {
                await Shell.Current.DisplayAlert("Résultat", "Aucun livre trouvé.", "OK");
            }

            Resultats = new ObservableCollection<Livre>(livres);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erreur dans SearchAsync : {ex.Message}");
        }
    }

    // Réinitialise tous les champs de recherche
    [RelayCommand]
    private void Reset()
    {
        Debug.WriteLine("Réinitialisation des filtres");
        TitreRecherche = string.Empty;
        NewTag = string.Empty;
        Tags.Clear();
        DateAjout = null;
        Resultats.Clear();
    }

    // Navigation vers la page de détails du livre sélectionné
    [RelayCommand]
    public async Task SelectLivreAsync(Livre selected)
    {
        if (selected is null)
            return;

        Debug.WriteLine($"Livre sélectionné : {selected.Titre} (ID: {selected.Livre_Id})");
        await Shell.Current.GoToAsync($"DetailsPage?id={selected.Livre_Id}");
    }
}
