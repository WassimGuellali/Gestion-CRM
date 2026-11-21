using System;
using System.Globalization;

namespace Crm.Ui
{
    /// <summary>Petites méthodes pour lire et vérifier les saisies au clavier.</summary>
    public static class Saisie
    {
        /// <summary>Lit un texte. Si defaut est fourni, Entrée conserve la valeur actuelle.</summary>
        public static string Texte(string invite, string defaut = null)
        {
            if (defaut == null) Console.Write(invite + " : ");
            else Console.Write(invite + " [" + defaut + "] : ");
            string saisie = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(saisie)) return defaut ?? "";
            return saisie.Trim();
        }

        public static int Entier(string invite, int? defaut = null)
        {
            while (true)
            {
                string texte = Texte(invite, defaut.HasValue ? defaut.Value.ToString() : null);
                int valeur;
                if (int.TryParse(texte, out valeur)) return valeur;
                Console.WriteLine("Veuillez saisir un nombre entier.");
            }
        }

        /// <summary>Lit un entier compris entre min et max (inclus).</summary>
        public static int Entier(string invite, int min, int max, int? defaut = null)
        {
            while (true)
            {
                int valeur = Entier(invite + " (" + min + "-" + max + ")", defaut);
                if (valeur >= min && valeur <= max) return valeur;
                Console.WriteLine("La valeur doit être comprise entre " + min + " et " + max + ".");
            }
        }

        public static decimal Montant(string invite, decimal? defaut = null)
        {
            while (true)
            {
                string texte = Texte(invite, defaut.HasValue ? defaut.Value.ToString(CultureInfo.InvariantCulture) : null);
                decimal valeur;
                if (decimal.TryParse(texte.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out valeur)
                    && valeur >= 0) return valeur;
                Console.WriteLine("Veuillez saisir un montant valide (ex : 12000 ou 12000.50).");
            }
        }

        public static DateTime Date(string invite, DateTime? defaut = null)
        {
            while (true)
            {
                string texte = Texte(invite + " (jj/mm/aaaa)", defaut.HasValue ? defaut.Value.ToString("dd/MM/yyyy") : null);
                DateTime valeur;
                if (DateTime.TryParseExact(texte, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out valeur))
                    return valeur;
                Console.WriteLine("Date invalide. Format attendu : jj/mm/aaaa.");
            }
        }

        public static bool Confirmer(string question)
        {
            Console.Write(question + " (o/n) : ");
            string reponse = Console.ReadLine();
            return reponse != null && reponse.Trim().ToLower().StartsWith("o");
        }

        public static void Pause()
        {
            Console.WriteLine();
            Console.Write("Appuyez sur Entrée pour continuer...");
            Console.ReadLine();
        }
    }
}
