using System;
using System.Collections.Generic;

namespace Crm.Model
{
    /// <summary>Opportunité commerciale. Elle contient 0..* activités.</summary>
    public class Opportunite
    {
        private readonly List<Activite> _activites = new List<Activite>();

        public int Id { get; private set; }
        public string Titre { get; set; }
        public decimal Montant { get; set; }
        public int Probabilite { get; set; }   // en pourcentage (0 à 100)
        public DateTime DateCloture { get; set; }
        public Etape Etape { get; set; }

        /// <summary>Compte concerné (renseigné par Compte.AjouterOpportunite).</summary>
        public Compte Compte { get; internal set; }

        /// <summary>Utilisateur responsable (renseigné par Utilisateur.AjouterOpportunite).</summary>
        public Utilisateur Responsable { get; internal set; }

        public IReadOnlyList<Activite> Activites
        {
            get { return _activites.AsReadOnly(); }
        }

        public Opportunite(int id, string titre, decimal montant, int probabilite, DateTime dateCloture, Etape etape)
        {
            Id = id;
            Titre = titre;
            Montant = montant;
            Probabilite = probabilite;
            DateCloture = dateCloture;
            Etape = etape;
        }

        public void AjouterActivite(Activite activite)
        {
            if (activite == null || _activites.Contains(activite)) return;
            _activites.Add(activite);
            activite.Opportunite = this;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} € ({2}, {3} %)", Titre, Montant, Etape, Probabilite);
        }
    }
}
