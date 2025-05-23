using System.Net.Http;
using System.Text;
using System.Text.Json;
using MauiApp1.Models;

namespace MauiApp1.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        // Constructeur sans paramètre — compatible avec instanciation manuelle
        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://10.0.2.2:3000/");
        }

        // Récupère tous les livres
        public async Task<List<Livre>> GetLivresAsync()
        {
            var response = await _httpClient.GetAsync("livres");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erreur API (GET livres): {response.StatusCode}");

            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(content, _jsonOptions);

            return apiResponse?.Data ?? new List<Livre>();
        }

        // Récupère un livre par ID
        public async Task<Livre?> GetLivreByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"livres/{id}");

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Livre>(content, _jsonOptions);
        }

        // Ajoute un nouveau livre
        public async Task<bool> PostLivreAsync(Livre livre)
        {
            var json = JsonSerializer.Serialize(livre, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("livres", content);
            return response.IsSuccessStatusCode;
        }

        // Modifie un livre existant
        public async Task<bool> PutLivreAsync(int id, Livre livre)
        {
            var json = JsonSerializer.Serialize(livre, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"livres/{id}", content);
            return response.IsSuccessStatusCode;
        }

        // Supprime un livre
        public async Task<bool> DeleteLivreAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"livres/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Livre>> SearchLivresAsync(string? titre = null, string? tag = null, DateTime? date = null)
        {
            var query = new List<string>();

            if (!string.IsNullOrWhiteSpace(titre))
                query.Add($"titre={Uri.EscapeDataString(titre)}");

            if (!string.IsNullOrWhiteSpace(tag))
                query.Add($"tag={Uri.EscapeDataString(tag)}");

            if (date.HasValue)
                query.Add($"date={date.Value:yyyy-MM-dd}");

            var url = "livres/search";
            if (query.Count > 0)
                url += "?" + string.Join("&", query);

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Erreur lors de la recherche.");

            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(content, _jsonOptions);

            return apiResponse?.Data ?? new List<Livre>();
        }

    }
}
