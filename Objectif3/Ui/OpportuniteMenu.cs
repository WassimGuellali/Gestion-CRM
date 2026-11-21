using System;
using System.Linq;
using Crm.Data;
using Crm.Model;
using Microsoft.EntityFrameworkCore;

namespace Crm.Ui
{
    /// <summary>Interface console : créer, lire, modifier et supprimer des opportunités.</summary>
    public static class OpportuniteMenu
    {
        public static void Afficher(CrmContext db)
        {
            bool retour = false;
            while (!retour)
            {
                Console.Clear();
                Console.WriteLine("===== GESTION DES OPPORTUNITÉS =====");
                Console.WriteLine("1. Lister les opportunités");
                Console.WriteLine("2. Créer une opportunité");
                Console.WriteLine("3. Modifier une opportunité");
                Console.WriteLine("4. Supprimer une opportunité");
                Console.WriteLine("0. Retour");
                Console.WriteLine();

                switch (Saisie.Texte("Votre choix"))
                {
                    case "1": Lister(db); Saisie.Pause(); break;
                    case "2": Creer(db); Saisie.Pause(); break;
                    case "3": Modifier(db); Saisie.Pause(); break;
                    case "4": Supprimer(db); Saisie.Pause(); break;
                    case "0": retour = true; break;
                    default:
                        Console.WriteLine("Choix invalide.");
                        Saisie.Pause();
                        break;
                }
            }
        }

        public static void Lister(CrmContext db)
        {
            var opportunites = db.Opportunites
                .Include(o => o.Compte)
                .Include(o => o.Responsable)
                .OrderBy(o => o.DateCloture)
                .ToList();

            Console.WriteLine();
            if (opportunites.Count == 0)
            {
                Console.WriteLine("Aucune opportunité enregistrée.");
                return;
            }

            decimal pipeline = 0;
            Console.WriteLine(opportunites.Count + " opportunité(s) :");
            foreach (Opportunite o in opportunites)
            {
                string compte = o.Compte != null ? o.Compte.Nom : "(sans compte)";
                string responsable = o.Responsable != null ? o.Responsable.Nom : "(non assignée)";
                Console.WriteLine("  [" + o.Id + "] " + o.Titre + " | " + compte + " | " + o.Montant + " TND | "
                                  + o.Etape + " (" + o.Probabilite + " %) | clôture " + o.DateCloture.ToString("dd/MM/yyyy")
                                  + " | " + responsable);
                pipeline += o.Montant;
            }
            Console.WriteLine("Pipeline total : " + pipeline + " TND");
        }

        private static Etape ChoisirEtape(Etape? actuelle = null)
        {
            Etape[] etapes = (Etape[])Enum.GetValues(typeof(Etape));
            for (int i = 0; i < etapes.Length; i++)
            {
                Console.WriteLine("  " + (i + 1) + ". " + etapes[i]);
            }
            int defaut = actuelle.HasValue ? Array.IndexOf(etapes, actuelle.Value) + 1 : 1;
            int choix = Saisie.Entier("Étape", 1, etapes.Length, defaut);
            return etapes[choix - 1];
        }

        private static void Creer(CrmContext db)
        {
            Console.WriteLine();
            CompteMenu.Lister(db);
            Console.WriteLine();
            int idCompte = Saisie.Entier("Identifiant du compte concerné");
            Compte compte = db.Comptes.Find(idCompte);
            if (compte == null)
            {
                Console.WriteLine("Compte introuvable : une opportunité doit concerner un compte.");
                return;
            }

            Console.WriteLine();
            foreach (Utilisateur u in db.Utilisateurs.OrderBy(ut => ut.Nom))
            {
                Console.WriteLine("  Utilisateur [" + u.Id + "] " + u);
            }
            int idResponsable = Saisie.Entier("Identifiant du responsable (0 = aucun)", 0);
            Utilisateur responsable = idResponsable == 0 ? null : db.Utilisateurs.Find(idResponsable);
            if (idResponsable != 0 && responsable == null)
            {
                Console.WriteLine("Utilisateur introuvable.");
                return;
            }

            string titre = Saisie.Texte("Titre");
            if (titre == "")
            {
                Console.WriteLine("Le titre est obligatoire.");
                return;
            }
            decimal montant = Saisie.Montant("Montant (TND)");
            int probabilite = Saisie.Entier("Probabilité en %", 0, 100);
            DateTime dateCloture = Saisie.Date("Date de clôture prévue");
            Etape etape = ChoisirEtape();

            Opportunite opportunite = new Opportunite(0, titre, montant, probabilite, dateCloture, etape);
            compte.AjouterOpportunite(opportunite);
            if (responsable != null) responsable.AjouterOpportunite(opportunite);
            db.Opportunites.Add(opportunite);
            db.SaveChanges();
            Console.WriteLine("Opportunité créée avec l'identifiant " + opportunite.Id + ".");
        }

        private static void Modifier(CrmContext db)
        {
            Lister(db);
            Console.WriteLine();
            int id = Saisie.Entier("Identifiant de l'opportunité à modifier");
            Opportunite o = db.Opportunites.Find(id);
            if (o == null)
            {
                Console.WriteLine("Opportunité introuvable.");
                return;
            }

            Console.WriteLine("(Appuyez sur Entrée pour garder la valeur actuelle)");
            o.Titre = Saisie.Texte("Titre", o.Titre);
            o.Montant = Saisie.Montant("Montant (TND)", o.Montant);
            o.Probabilite = Saisie.Entier("Probabilité en %", 0, 100, o.Probabilite);
            o.DateCloture = Saisie.Date("Date de clôture prévue", o.DateCloture);
            o.Etape = ChoisirEtape(o.Etape);
            db.SaveChanges();
            Console.WriteLine("Opportunité modifiée.");
        }

        private static void Supprimer(CrmContext db)
        {
            Lister(db);
            Console.WriteLine();
            int id = Saisie.Entier("Identifiant de l'opportunité à supprimer");
            Opportunite o = db.Opportunites.Find(id);
            if (o == null)
            {
                Console.WriteLine("Opportunité introuvable.");
                return;
            }

            if (Saisie.Confirmer("Supprimer « " + o.Titre + " » avec ses activités ?"))
            {
                db.Opportunites.Remove(o);
                db.SaveChanges();
                Console.WriteLine("Opportunité supprimée.");
            }
            else
            {
                Console.WriteLine("Suppression annulée.");
            }
        }
    }
}
