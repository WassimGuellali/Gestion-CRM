using System;
using System.Linq;
using Crm.Data;
using Crm.Model;
using Microsoft.EntityFrameworkCore;

namespace Crm.Ui
{
    /// <summary>Interface console : créer, lire, modifier et supprimer des comptes.</summary>
    public static class CompteMenu
    {
        public static void Afficher(CrmContext db)
        {
            bool retour = false;
            while (!retour)
            {
                Console.Clear();
                Console.WriteLine("===== GESTION DES COMPTES =====");
                Console.WriteLine("1. Lister les comptes");
                Console.WriteLine("2. Créer un compte");
                Console.WriteLine("3. Modifier un compte");
                Console.WriteLine("4. Supprimer un compte");
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
            var comptes = db.Comptes
                .Include(c => c.Contacts)
                .Include(c => c.Opportunites)
                .OrderBy(c => c.Nom)
                .ToList();

            Console.WriteLine();
            if (comptes.Count == 0)
            {
                Console.WriteLine("Aucun compte enregistré.");
                return;
            }
            Console.WriteLine(comptes.Count + " compte(s) :");
            foreach (Compte c in comptes)
            {
                Console.WriteLine("  [" + c.Id + "] " + c + " | " + c.Telephone + " | " + c.SiteWeb
                                  + " | " + c.Contacts.Count + " contact(s), " + c.Opportunites.Count + " opportunité(s)");
            }
        }

        private static void Creer(CrmContext db)
        {
            Console.WriteLine();
            string nom = Saisie.Texte("Nom");
            if (nom == "")
            {
                Console.WriteLine("Le nom est obligatoire.");
                return;
            }
            string secteur = Saisie.Texte("Secteur");
            string siteWeb = Saisie.Texte("Site web");
            string telephone = Saisie.Texte("Téléphone");
            string ville = Saisie.Texte("Ville");

            Compte compte = new Compte(0, nom, secteur, siteWeb, telephone, ville);
            db.Comptes.Add(compte);
            db.SaveChanges();
            Console.WriteLine("Compte créé avec l'identifiant " + compte.Id + ".");
        }

        private static void Modifier(CrmContext db)
        {
            Lister(db);
            Console.WriteLine();
            int id = Saisie.Entier("Identifiant du compte à modifier");
            Compte compte = db.Comptes.Find(id);
            if (compte == null)
            {
                Console.WriteLine("Compte introuvable.");
                return;
            }

            Console.WriteLine("(Appuyez sur Entrée pour garder la valeur actuelle)");
            compte.Nom = Saisie.Texte("Nom", compte.Nom);
            compte.Secteur = Saisie.Texte("Secteur", compte.Secteur);
            compte.SiteWeb = Saisie.Texte("Site web", compte.SiteWeb);
            compte.Telephone = Saisie.Texte("Téléphone", compte.Telephone);
            compte.Ville = Saisie.Texte("Ville", compte.Ville);
            db.SaveChanges();
            Console.WriteLine("Compte modifié.");
        }

        private static void Supprimer(CrmContext db)
        {
            Lister(db);
            Console.WriteLine();
            int id = Saisie.Entier("Identifiant du compte à supprimer");
            Compte compte = db.Comptes.Find(id);
            if (compte == null)
            {
                Console.WriteLine("Compte introuvable.");
                return;
            }

            if (Saisie.Confirmer("Supprimer « " + compte.Nom + " » avec ses contacts et ses opportunités ?"))
            {
                db.Comptes.Remove(compte);
                db.SaveChanges();
                Console.WriteLine("Compte supprimé.");
            }
            else
            {
                Console.WriteLine("Suppression annulée.");
            }
        }
    }
}
