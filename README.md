# README: Simulation de Billes

## Introduction

Ce projet simule le comportement de différentes billes en utilisant des principes physiques réalistes. Les billes sont affectées par la gravité, le frottement et les collisions. De plus, des effets visuels de liquide sont intégrés grâce à un shader spécialisé.

## Types de Billes

Le simulateur comprend quatre types de billes :

1. **Bille Normale :**
   - Comportement classique soumis à la gravité et aux collisions.
  
2. **Bille avec Mini-Billes :**
   - Contient plusieurs petites billes à l'intérieur, chacune interagissant avec les autres.

3. **Bille à Collision de Choc :**
   - Subit des collisions inélastiques, entraînant une perte d'énergie cinétique.

4. **Bille à Simulation d'Eau :**
   - Se déplace dans un fluide, subissant des forces de traînée.

## Shader de Liquide

### Liquid Shader

Le **Liquid Shader**, créé par **Joyce [MinionsArt]**, est utilisé pour simuler des effets visuels réalistes de l'eau. 

- **Twitter :** [@minionsart](https://twitter.com/minionsart)
- **Patreon :** [MinionsArt](http://www.patreon.com/minionsart)

## Calculs Physiques

Les calculs physiques suivants sont intégrés dans la simulation :

### A. Gravité

La force de gravité \( \vec{F}_g \) agissant sur une bille est donnée par :

\[ \vec{F}_g = m \vec{g} \]

où \( m \) est la masse de la bille et \( \vec{g} \) est l'accélération due à la gravité (environ \( 9.81 \, m/s^2 \)).

### B. Frottement

La force de frottement \( \vec{F}_f \) est modélisée par :

\[ \vec{F}_f = -b \vec{v} \]

où \( b \) est le coefficient de frottement et \( \vec{v} \) est la vitesse de la bille.

### C. Collisions

1. **Collisions Élastiques :**
   - La conservation de la quantité de mouvement et de l'énergie est appliquée.
  
   \[
   m_1 \vec{v}_{1i} + m_2 \vec{v}_{2i} = m_1 \vec{v}_{1f} + m_2 \vec{v}_{2f}
   \]

2. **Collisions Inélastiques :**
   - Utilisation d'un coefficient de restitution \( e \) pour modéliser la perte d'énergie.

   \[
   \vec{v}_{1f} - \vec{v}_{2f} = -e (\vec{v}_{1i} - \vec{v}_{2i})
   \]

3. **Collisions avec Mini-Billes :**
   - Similaires aux collisions élastiques ou inélastiques selon le type de bille.

4. **Collisions dans un Fluide :**
   - La force de traînée \( \vec{F}_d \) est calculée pour simuler l'effet du fluide sur le mouvement de la bille.

### D. Intégration Numérique

Une méthode d'intégration numérique (Euler ou Runge-Kutta) est utilisée pour mettre à jour la position et la vitesse des billes à chaque pas de temps.