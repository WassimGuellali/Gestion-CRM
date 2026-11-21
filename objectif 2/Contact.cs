using System.Collections.Generic;

namespace Crm.Model
{
    /// <summary>Contact appartenant à un compte. Il peut être impliqué dans 0..* activités.</summary>
    public class Contact
    {
        private readonly List<Activite> _activites = new List<Activite>();

        public int Id { get; private set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Poste { get; set; }

        /// <summary>Compte auquel appartient le contact (renseigné par Compte.AjouterContact).</summary>
        public Compte Compte { get; internal set; }

        public IReadOnlyList<Activite> Activites
        {
            get { return _activites.AsReadOnly(); }
        }

        public Contact(int id, string nom, string prenom, string email, string telephone, string poste)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Email = email;
            Telephone = telephone;
            Poste = poste;
        }

        public void AjouterActivite(Activite activite)
        {
            if (activite == null || _activites.Contains(activite)) return;
            _activites.Add(activite);
            activite.Contact = this;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} - {2}", Prenom, Nom, Poste);
        }
    }
}
