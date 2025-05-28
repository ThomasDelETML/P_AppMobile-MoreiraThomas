namespace ReadME.Models
{
    // Sert à recevoir les réponses de l'API : un message + les données
    public class ApiResponse<T>
    {
        public string? Message { get; set; }  // Message comme "succès" ou "erreur"
        public T? Data { get; set; }          // Les données reçues (ex : liste de livres)
    }
}
