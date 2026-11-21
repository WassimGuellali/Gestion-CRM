using System;
using System.Linq;
using Crm.Data;
using Crm.Model;
using Microsoft.EntityFrameworkCore;

namespace Crm.Ui
{
    /// <summary>Interface console : créer, lire, modifier et supprimer des contacts.</summary>
    public static class ContactMenu
    {
        public static void Afficher(CrmContext db)
        {
            bool retour = false;
            while (!retour)
            {
                Console.Clear();
                Console.WriteLine("===== GESTION DES CONTACTS =====");
                Console.WriteLine("1. Lister les contacts");
                Console.WriteLine("2. Créer un contact");
                Console.WriteLine("3. Modifier un contact");
                Console.WriteLine("4. Supprimer un contact");
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
            var contacts = db.Contacts
                .Include(c => c.Compte)
                .OrderBy(c => c.Nom)
                .ToList();

            Console.WriteLine();
            if (contacts.Count == 0)
            {
                Console.WriteLine("Aucun contact enregistré.");
                return;
            }
            Console.WriteLine(contacts.Count + " contact(s) :");
            foreach (Contact c in contacts)
            {
                string compte = c.Compte != null ? c.Compte.Nom : "(sans compte)";
                Console.WriteLine("  [" + c.Id + "] " + c.Prenom + " " + c.Nom + " | " + c.Email
                                  + " | " + c.Telephone + " | " + c.Poste + " | " + compte);
            }
        }

        private static void Creer(CrmContext db)
        {
            Console.WriteLine();
            CompteMenu.Lister(db);
            Console.WriteLine();
            int idCompte = Saisie.Entier("Identifiant du compte du contact");
            Compte compte = db.Comptes.Find(idCompte);
            if (compte == null)
            {
                Console.WriteLine("Compte introuvable : un contact doit appartenir à un compte.");
                return;
            }

            string nom = Saisie.Texte("Nom");
            if (nom == "")
            {
                Console.WriteLine("Le nom est obligatoire.");
                return;
            }
            string prenom = Saisie.Texte("Prénom");
            string email = Saisie.Texte("Email");
            string telephone = Saisie.Texte("Téléphone");
            string poste = Saisie.Texte("Poste");

            Contact contact = new Contact(0, nom, prenom, email, telephone, poste);
            compte.AjouterContact(contact);
            db.Contacts.Add(contact);
            db.SaveChanges();
            Console.WriteLine("Contact créé avec l'identifiant " + contact.Id + ".");
        }

        private static void Modifier(CrmContext db)
        {
            Lister(db);
            Console.WriteLine();
            int id = Saisie.Entier("Identifiant du contact à modifier");
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                Console.WriteLine("Contact introuvable.");
                return;
            }

            Console.WriteLine("(Appuyez sur Entrée pour garder la valeur actuelle)");
            contact.Nom = Saisie.Texte("Nom", contact.Nom);
            contact.Prenom = Saisie.Texte("Prénom", contact.Prenom);
            contact.Email = Saisie.Texte("Email", contact.Email);
            contact.Telephone = Saisie.Texte("Téléphone", contact.Telephone);
            contact.Poste = Saisie.Texte("Poste", contact.Poste);
            db.SaveChanges();
            Console.WriteLine("Contact modifié.");
        }

        private static void Supprimer(CrmContext db)
        {
            Lister(db);
            Console.WriteLine();
            int id = Saisie.Entier("Identifiant du contact à supprimer");
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                Console.WriteLine("Contact introuvable.");
                return;
            }

            if (Saisie.Confirmer("Supprimer « " + contact.Prenom + " " + contact.Nom + " » ?"))
            {
                db.Contacts.Remove(contact);
                db.SaveChanges();
                Console.WriteLine("Contact supprimé.");
            }
            else
            {
                Console.WriteLine("Suppression annulée.");
            }
        }
    }
}
