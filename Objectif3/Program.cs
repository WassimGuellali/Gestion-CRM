using System;
using System.Linq;
using Crm.Data;
using Crm.Ui;
using Microsoft.EntityFrameworkCore;

namespace Crm
{
    /// <summary>
    /// Application console du CRM : le modèle objet est enregistré dans une base SQLite
    /// grâce à Entity Framework Core (Code First + migrations).
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            using (CrmContext db = new CrmContext())
            {
                if (!db.Database.GetMigrations().Any())
                {
                    Console.WriteLine("Aucune migration trouvée. Dans le dossier src, exécutez :");
                    Console.WriteLine("  dotnet ef migrations add InitialCreate");
                    Console.WriteLine("  dotnet ef database update");
                    return;
                }

                // Applique les migrations : crée ou met à jour crm.db
                db.Database.Migrate();

                // Ajoute des données d'exemple si la base est vide
                Donnees.InitialiserSiVide(db);

                MenuPrincipal.Afficher(db);
            }
        }
    }
}
