# P_AppMobile-MoreiraThomas – Procédure d’installation

## Prérequis

- Git
- Node.js (v14+ recommandé)
- Docker Desktop
- Emulateur Android ETML
- Visual Studio 2022 + Extensions MAUI et MVVM Toolkit
- Visual Studio Code

---

## 1. Cloner le dépôt

- Exécutez

```bash
git clone https://github.com/ThomasDelETML/P_AppMobile-MoreiraThomas.git
```

## 2. Démarrer MySQL avec Docker et créer la database

- Lancez une console dans le dossier principal du projet et lancez cettes commandes

```bash
cd Backend/Docker_MySQL
docker compose up -d
docker exec -it db_readme mysql -u root -proot
CREATE DATABASE IF NOT EXISTS db_livre;
exit

```

## 3. Importer les Epubs dans la database

- Dans la même console, lancez cettes commandes

```bash
cd scripts
node importEpubs.mjs

```

---

## 4. Lancer le backend

- Dans la même console, lancez le backend avec cettes commandes

```bash
cd ..
cd ..
npm install
npm start

```

Votre backend est prêt à être utilisé par votre frontend MAUI

---

## 5. Vérification avant de lancer le frontend MAUI

Liste de chooses à vérifier

- Lancez votre emulateur et activez le mode de dépuration
- Assurez vous de bien installer le ToolKit de MVVM

---

## 6. Lancez votre application

- Voilà tout prét. Vous pouvez expériencer mon application.

---

### Moreira Thomas - 28.05.2025 - po51oro@eduvaud.ch
