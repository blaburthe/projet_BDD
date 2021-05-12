using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace veloMax
{
    class ItemCommande
    {

        private string idItem;
        private string description;
        string type; //piece ou velo
        int prixUClient;
        int quantite;
        int delai;

        public ItemCommande(string numeroCatalogue, string description, int prixU,int delai, string type,int quantite = 1)
        {
            this.idItem = numeroCatalogue;
            this.description = description;
            this.prixUClient = prixU;
            this.quantite = quantite;
            this.delai = delai;
            this.type = type;
        }

        public string IdItem { get { return idItem; } }
        public string Description { get { return description; } }
        public int PrixUClient { get { return prixUClient; } }
        public int Delai { get { return delai; } }
        public int Quantite { get { return quantite; } }
        public string Type { get { return type; } }

       




    }
}
