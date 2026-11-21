using System.Collections.Generic;

namespace Crm.Model
{
    /// <summary>Compte client. Un compte emploie 0..* contacts et concerne 0..* opportunités.</summary>
    public class Compte
    {
        private readonly List<Contact> _contacts = new List<Contact>();
        private readonly List<Opportunite> _opportunites = new List<Opportunite>();

        public int Id { get; private set; }
        public string Nom { get; set; }
        public string Secteur { get; set; }
        public string SiteWeb { get; set; }
        public string Telephone { get; set; }
        public string Ville { get; set; }

        public IReadOnlyList<Contact> Contacts
        {
            get { return _contacts.AsReadOnly(); }
        }

        public IReadOnlyList<Opportunite> Opportunites
        {
            get { return _opportunites.AsReadOnly(); }
        }

        public Compte(int id, string nom, string secteur, string siteWeb, string telephone, string ville)
        {
            Id = id;
            Nom = nom;
            Secteur = secteur;
            SiteWeb = siteWeb;
            Telephone = telephone;
            Ville = ville;
        }

        public void AjouterContact(Contact contact)
        {
            if (contact == null || _contacts.Contains(contact)) return;
            _contacts.Add(contact);
            contact.Compte = this;
        }

        public void AjouterOpportunite(Opportunite opportunite)
        {
            if (opportunite == null || _opportunites.Contains(opportunite)) return;
            _opportunites.Add(opportunite);
            opportunite.Compte = this;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1}, {2})", Nom, Secteur, Ville);
        }
    }
}
