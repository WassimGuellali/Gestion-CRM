using System.Collections.Generic;

namespace Crm.Model
{
    /// <summary>Utilisateur du CRM. Un utilisateur est responsable de 0..* opportunités.</summary>
    public class Utilisateur
    {
        private readonly List<Opportunite> _opportunites = new List<Opportunite>();

        public int Id { get; private set; }
        public string Nom { get; set; }
        public string Email { get; set; }
        public string MotDePasse { get; set; }
        public Role Role { get; set; }

        public IReadOnlyList<Opportunite> Opportunites
        {
            get { return _opportunites.AsReadOnly(); }
        }

        public Utilisateur(int id, string nom, string email, string motDePasse, Role role)
        {
            Id = id;
            Nom = nom;
            Email = email;
            MotDePasse = motDePasse;
            Role = role;
        }

        /// <summary>Associe une opportunité à cet utilisateur (met à jour les deux côtés).</summary>
        public void AjouterOpportunite(Opportunite opportunite)
        {
            if (opportunite == null || _opportunites.Contains(opportunite)) return;
            _opportunites.Add(opportunite);
            opportunite.Responsable = this;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Nom, Role);
        }
    }
}
