using System;
using Crm.Model;

namespace Crm
{
    /// <summary>Petit programme de démonstration du modèle objet.</summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            // Utilisateur et compte
            Utilisateur sara = new Utilisateur(1, "Sara Ben Ali", "sara@crm.tn", "motdepasse", Role.Commercial);
            Compte compte = new Compte(1, "Entreprise Exemple", "Informatique", "www.exemple.tn", "71 000 000", "Tunis");

            // Contact rattaché au compte
            Contact contact = new Contact(1, "Trabelsi", "Karim", "karim@exemple.tn", "20 000 000", "Directeur achats");
            compte.AjouterContact(contact);

            // Opportunité liée au compte et à l'utilisateur
            Opportunite opp = new Opportunite(1, "Déploiement CRM", 25000m, 60, new DateTime(2027, 3, 31), Etape.Proposition);
            compte.AjouterOpportunite(opp);
            sara.AjouterOpportunite(opp);

            // Activités : une avec contact, une note interne sans contact
            Activite appel = new Activite(1, "Appel de qualification", "Besoins du client", TypeActivite.Appel, DateTime.Now);
            Activite note = new Activite(2, "Note interne", "Préparer la proposition", TypeActivite.Note, DateTime.Now);
            opp.AjouterActivite(appel);
            opp.AjouterActivite(note);
            contact.AjouterActivite(appel);

            // Affichage
            Console.WriteLine("Compte      : " + compte);
            Console.WriteLine("Responsable : " + opp.Responsable);
            Console.WriteLine("Opportunité : " + opp);
            Console.WriteLine("Contacts du compte :");
            foreach (Contact c in compte.Contacts) Console.WriteLine("  - " + c);
            Console.WriteLine("Activités de l'opportunité :");
            foreach (Activite a in opp.Activites)
            {
                string avec = a.Contact != null ? a.Contact.ToString() : "(aucun contact)";
                Console.WriteLine("  - " + a + " avec " + avec);
            }
        }
    }
}
