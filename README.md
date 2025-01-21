# Projet Unity : Sphère dans une Sphère

## Description
Ce projet Unity démontre la création d'une sphère à l'intérieur d'une autre sphère, avec des collisions gérées. L'objectif est de fournir une base pour explorer la physique et les interactions dans Unity.

## Prérequis
- Unity Hub (dernière version recommandée)
- Unity Editor (version **2020.3 LTS** ou supérieure)

## Installation

1. **Cloner le dépôt :**
   ```bash
   git clone <URL_DU_DEPOT>
   ```

2. **Ouvrir le projet :**
   - Lancez Unity Hub.
   - Cliquez sur **Add** et sélectionnez le dossier cloné pour l'ajouter à votre liste de projets.

3. **Ouvrir la scène :**
   - Dans le **Project** panel, naviguez vers `Assets/Scenes`.
   - Double-cliquez sur la scène `MainScene` pour l'ouvrir.

## Utilisation

1. **Jouer la scène :**
   - Cliquez sur le bouton **Play** en haut de l'éditeur pour démarrer la simulation.
   - Observez les interactions entre les sphères.

2. **Modifier les sphères :**
   - Vous pouvez redimensionner ou déplacer les sphères dans la hiérarchie pour voir comment cela affecte les collisions.

## Scripts et Fonctionnalités

- **CollisionHandler.cs** : Gère les événements de collision entre les sphères. 
- Utilise la méthode `OnCollisionEnter` pour détecter et afficher un message dans la console lors d'une collision.

## Auteurs
- DVMAX18 - Développeur principal

## License
Ce projet est sous la **MIT License** - voir le fichier [LICENSE](LICENSE) pour plus de détails.
