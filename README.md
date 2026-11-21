# Gestion-CRM

Application de gestion de la relation client (CRM) développée en **C#** avec une approche **orientée objet**.

Le CRM permet à une entreprise de gérer ses **comptes (clients)**, leurs **contacts**, ses **opportunités commerciales** et le suivi des **activités** (appels, emails, réunions, notes). Les droits d'accès dépendent du rôle de l'utilisateur : **Admin**, **Commercial** ou **Lecteur**.
## Structure du dépôt

- `objectif 2/` : conception et programmation orientée objet (classes C#)
- `Objectif3/` : intégration d'une base de données SQLite avec Entity Framework Core (Code First, migrations) et interfaces console CRUD
- `docs/` : compte rendu et diagrammes de conception

Pour lancer l'objectif 3 : `cd Objectif3` puis `dotnet run`.
## Documentation

- [Compte rendu de conception (PDF)](docs/compte-rendu.pdf) : cas d'utilisation, droits par rôle, diagramme de classes.

## Modèle

| Classe | Description |
|---|---|
| `Utilisateur` | Utilisateur de l'application, avec un `Role` |
| `Compte` | Entreprise cliente (nom, secteur, site web, téléphone, ville) |
| `Contact` | Personne rattachée à un compte |
| `Opportunite` | Affaire commerciale (montant, probabilité, date de clôture, étape) |
| `Activite` | Action de suivi (appel, email, réunion, note) |

Énumérations : `Role`, `Etape`, `TypeActivite`.

## Prochaines étapes

1. Couche de services (connexion, gestion des comptes, contacts, opportunités, activités, utilisateurs).
2. Calcul des indicateurs de rapport (chiffre d'affaires, taux de conversion, pipeline).
3. Persistance des données (base de données) et interface.

## Auteurs

Youssef Lazzez et Wassim Guellali, 2ème année GSIL « C », ENICarthage.
