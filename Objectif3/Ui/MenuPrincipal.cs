using System;
using Crm.Data;

namespace Crm.Ui
{
    public static class MenuPrincipal
    {
        public static void Afficher(CrmContext db)
        {
            bool quitter = false;
            while (!quitter)
            {
                Console.Clear();
                Console.WriteLine("========== CRM ==========");
                Console.WriteLine("1. Gestion des comptes");
                Console.WriteLine("2. Gestion des contacts");
                Console.WriteLine("3. Gestion des opportunités");
                Console.WriteLine("0. Quitter");
                Console.WriteLine();

                switch (Saisie.Texte("Votre choix"))
                {
                    case "1": CompteMenu.Afficher(db); break;
                    case "2": ContactMenu.Afficher(db); break;
                    case "3": OpportuniteMenu.Afficher(db); break;
                    case "0": quitter = true; break;
                    default:
                        Console.WriteLine("Choix invalide.");
                        Saisie.Pause();
                        break;
                }
            }
        }
    }
}
