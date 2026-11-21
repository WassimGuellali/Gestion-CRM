using System;
using System.IO;
using Crm.Model;
using Microsoft.EntityFrameworkCore;

namespace Crm.Data
{
    /// <summary>
    /// Contexte Entity Framework Core (approche Code First) :
    /// il décrit les tables de la base SQLite à partir des classes du modèle.
    /// </summary>
    public class CrmContext : DbContext
    {
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Compte> Comptes { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Opportunite> Opportunites { get; set; }
        public DbSet<Activite> Activites { get; set; }

        /// <summary>
        /// La base crm.db est créée à côté de Crm.csproj (dossier src),
        /// pour pouvoir la pousser facilement sur GitHub.
        /// </summary>
        public static string CheminBase()
        {
            DirectoryInfo dossier = new DirectoryInfo(AppContext.BaseDirectory);
            while (dossier != null && !File.Exists(Path.Combine(dossier.FullName, "Crm.csproj")))
            {
                dossier = dossier.Parent;
            }
            string racine = dossier != null ? dossier.FullName : Directory.GetCurrentDirectory();
            return Path.Combine(racine, "crm.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=" + CheminBase());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Compte 1 -> 0..* Contact : supprimer un compte supprime ses contacts
            modelBuilder.Entity<Compte>()
                .HasMany(c => c.Contacts).WithOne(c => c.Compte)
                .OnDelete(DeleteBehavior.Cascade);

            // Compte 1 -> 0..* Opportunité
            modelBuilder.Entity<Compte>()
                .HasMany(c => c.Opportunites).WithOne(o => o.Compte)
                .OnDelete(DeleteBehavior.Cascade);

            // Utilisateur 1 -> 0..* Opportunité : supprimer un utilisateur garde ses opportunités
            modelBuilder.Entity<Utilisateur>()
                .HasMany(u => u.Opportunites).WithOne(o => o.Responsable)
                .OnDelete(DeleteBehavior.SetNull);

            // Opportunité 1 -> 0..* Activité
            modelBuilder.Entity<Opportunite>()
                .HasMany(o => o.Activites).WithOne(a => a.Opportunite)
                .OnDelete(DeleteBehavior.Cascade);

            // Contact 0..1 -> 0..* Activité : supprimer un contact garde l'activité
            modelBuilder.Entity<Contact>()
                .HasMany(c => c.Activites).WithOne(a => a.Contact)
                .OnDelete(DeleteBehavior.SetNull);

            // Les collections du modèle sont exposées en lecture seule :
            // EF Core les remplit directement via les champs privés.
            modelBuilder.Entity<Compte>().Navigation(c => c.Contacts).HasField("_contacts");
            modelBuilder.Entity<Compte>().Navigation(c => c.Opportunites).HasField("_opportunites");
            modelBuilder.Entity<Contact>().Navigation(c => c.Activites).HasField("_activites");
            modelBuilder.Entity<Opportunite>().Navigation(o => o.Activites).HasField("_activites");
            modelBuilder.Entity<Utilisateur>().Navigation(u => u.Opportunites).HasField("_opportunites");

            // Les énumérations sont stockées en texte (plus lisible dans la base)
            modelBuilder.Entity<Utilisateur>().Property(u => u.Role).HasConversion<string>();
            modelBuilder.Entity<Opportunite>().Property(o => o.Etape).HasConversion<string>();
            modelBuilder.Entity<Activite>().Property(a => a.Type).HasConversion<string>();
        }
    }
}
