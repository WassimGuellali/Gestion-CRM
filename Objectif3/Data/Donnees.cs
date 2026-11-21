using System;
using System.Linq;
using Crm.Model;

namespace Crm.Data
{
    /// <summary>Données de démonstration insérées si la base est vide.</summary>
    public static class Donnees
    {
        public static void InitialiserSiVide(CrmContext db)
        {
            if (db.Comptes.Any() || db.Utilisateurs.Any()) return;

            // Id = 0 : c'est la base de données qui génère les identifiants
            Utilisateur admin = new Utilisateur(0, "Admin", "admin@crm.tn", "admin123", Role.Admin);
            Utilisateur sarah = new Utilisateur(0, "Sarah Abdelli", "sarah@crm.tn", "sarah123", Role.Commercial);

            Compte techCorp = new Compte(0, "TechCorp Tunisia", "Technologie", "www.techcorp.tn", "+216 71 123 456", "Tunis");
            Compte industrielle = new Compte(0, "Société Industrielle du Nord", "Industrie", "www.sin.com.tn", "+216 72 234 567", "Bizerte");

            Contact mohamed = new Contact(0, "Ben Ali", "Mohamed", "mohamed.benali@techcorp.tn", "+216 20 111 222", "Directeur technique");
            Contact fatma = new Contact(0, "Trabelsi", "Fatma", "fatma.trabelsi@techcorp.tn", "+216 20 333 444", "Responsable achats");
            Contact ahmed = new Contact(0, "Mansour", "Ahmed", "ahmed.mansour@sin.com.tn", "+216 20 555 666", "Directeur général");
            techCorp.AjouterContact(mohamed);
            techCorp.AjouterContact(fatma);
            industrielle.AjouterContact(ahmed);

            Opportunite siteWeb = new Opportunite(0, "Développement site e-commerce", 12000m, 100, new DateTime(2026, 3, 30), Etape.Gagnee);
            Opportunite appMobile = new Opportunite(0, "Application mobile iOS/Android", 35000m, 40, new DateTime(2026, 6, 30), Etape.Qualification);
            Opportunite formation = new Opportunite(0, "Formation développement web", 8000m, 20, new DateTime(2026, 5, 1), Etape.Prospection);
            techCorp.AjouterOpportunite(siteWeb);
            techCorp.AjouterOpportunite(appMobile);
            industrielle.AjouterOpportunite(formation);
            sarah.AjouterOpportunite(siteWeb);
            sarah.AjouterOpportunite(appMobile);
            sarah.AjouterOpportunite(formation);

            Activite appel = new Activite(0, "Discussion sur le projet e-commerce",
                "Appel pour discuter des spécifications techniques", TypeActivite.Appel, new DateTime(2026, 3, 24, 10, 0, 0));
            siteWeb.AjouterActivite(appel);
            mohamed.AjouterActivite(appel);

            db.Utilisateurs.AddRange(admin, sarah);
            db.Comptes.AddRange(techCorp, industrielle);
            db.SaveChanges();
        }
    }
}
