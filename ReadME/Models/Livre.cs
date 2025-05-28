namespace ReadME.Models
{
    // Représente un livre avec ses infos
    public class Livre
    {
        public int Livre_Id { get; set; }         // Identifiant unique
        public string? Uuid { get; set; }         // Identifiant global unique
        public string? Titre { get; set; }        // Titre du livre
        public string? Auteur { get; set; }       // Nom de l’auteur
        public string? Langue { get; set; }       // Langue du livre
        public string? Sujet { get; set; }        // Sujet ou thème
        public DateTime? Date_Modification { get; set; } // Date de la dernière modif
        public string? Resume { get; set; }       // Résumé du livre (non utilisé ici)
        public string? Contenu { get; set; }      // Texte complet du livre
        public DateTime CreatedAt { get; set; }   // Date d’ajout dans la base

        public string? ImageCouverture { get; set; } // Image de couverture (base64)

        // Utilisé pour afficher l’image dans l’app à partir du texte encodé
        public ImageSource? ImageSource =>
            !string.IsNullOrEmpty(ImageCouverture)
                ? ImageSource.FromStream(() =>
                    new MemoryStream(Convert.FromBase64String(ImageCouverture)))
                : null;
    }
}
