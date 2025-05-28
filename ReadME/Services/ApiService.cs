using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using ReadME.Models;

namespace ReadME.Services
{
    // Service qui gère les appels à l'API backend
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        // Options pour désérialiser les noms en ignorant la casse
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiService()
        {
            // Adresse IP spéciale pour accéder à localhost depuis l'émulateur Android
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://10.0.2.2:3000/")
            };
        }

        // Récupère une liste de livres depuis l'API
        public async Task<List<Livre>> GetLivresAsync()
        {
            var response = await _httpClient.GetAsync("livres");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erreur API (GET livres): {response.StatusCode}");

            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<Livre>>>(content, _jsonOptions);

            return apiResponse?.Data ?? new List<Livre>();
        }

        // Récupère un livre en fonction de son ID
        public async Task<Livre?> GetLivreByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"livres/{id}");

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<Livre>>(content, _jsonOptions);

            return apiResponse?.Data;
        }

        // Envoie un nouveau livre à l'API
        public async Task<bool> PostLivreAsync(Livre livre)
        {
            var json = JsonSerializer.Serialize(livre, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("livres", content);

            return response.IsSuccessStatusCode;
        }

        // Met à jour un livre existant
        public async Task<bool> PutLivreAsync(int id, Livre livre)
        {
            var json = JsonSerializer.Serialize(livre, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"livres/{id}", content);

            return response.IsSuccessStatusCode;
        }

        // Supprime un livre en fonction de son ID
        public async Task<bool> DeleteLivreAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"livres/{id}");
            return response.IsSuccessStatusCode;
        }

        // Recherche des livres à partir d’un mot-clé (titre, auteur ou contenu)
        public async Task<List<Livre>> SearchLivresAsync(string query)
        {
            var encodedQuery = Uri.EscapeDataString(query);
            var response = await _httpClient.GetAsync($"livres/search?query={encodedQuery}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erreur recherche : {response.StatusCode}");

            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<Livre>>>(content, _jsonOptions);

            return apiResponse?.Data ?? new List<Livre>();
        }
    }
}
