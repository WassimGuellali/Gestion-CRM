using System;

namespace Crm.Model
{
    /// <summary>
    /// Activité de suivi liée à une opportunité. Elle peut concerner un contact (optionnel).
    /// </summary>
    public class Activite
    {
        public int Id { get; private set; }
        public string Titre { get; set; }
        public string Description { get; set; }
        public TypeActivite Type { get; set; }
        public DateTime DateHeure { get; set; }

        /// <summary>Opportunité suivie (renseignée par Opportunite.AjouterActivite).</summary>
        public Opportunite Opportunite { get; internal set; }

        /// <summary>Contact concerné, peut rester null (0..1), par exemple pour une note interne.</summary>
        public Contact Contact { get; internal set; }

        public Activite(int id, string titre, string description, TypeActivite type, DateTime dateHeure)
        {
            Id = id;
            Titre = titre;
            Description = description;
            Type = type;
            DateHeure = dateHeure;
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1} - {2:dd/MM/yyyy HH:mm}", Type, Titre, DateHeure);
        }
    }
}
