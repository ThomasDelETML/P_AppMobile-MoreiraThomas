using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MauiApp1
{
    public class Livre  
    {
        public int livre_id { get; set; }
        public string titre { get; set; }
        public string imageCouverturePath { get; set; }
        public int nbPage { get; set; }
        public int anneeEdition { get; set; }
        public string resume { get; set; }
        public int utilisateur_fk { get; set; }
        public int editeur_fk { get; set; }
        public int categorie_fk { get; set; }
        public int auteur_fk { get; set; }
    }

}
