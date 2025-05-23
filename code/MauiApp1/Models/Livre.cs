namespace MauiApp1.Models
{
    public class Livre
    {
        public int Livre_Id { get; set; }
        public string? Titre { get; set; }
        public string? ImageCouverturePath { get; set; }
        public int NbPage { get; set; }
        public int AnneeEdition { get; set; }
        public string? Resume { get; set; }
        public Auteur? Auteur { get; set; }
        public Categorie? Categorie { get; set; }

        public string ImageUrl => "http://10.0.2.2:3000" + ImageCouverturePath;
    }
}
