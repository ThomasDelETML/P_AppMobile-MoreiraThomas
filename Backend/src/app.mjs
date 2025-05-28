import express from "express";
import cors from "cors";
import { sequelize, initDb } from "./db/sequelize.mjs";
import { livreRouter } from "./routes/livre.mjs";

const app = express();
const port = 3000;

// Middleware globaux
app.use(cors());
app.use(express.json());

// Route de test
app.get("/", (req, res) => {
  res.send("API EPUB active");
});

// Routes principales
app.use("/livres", livreRouter);

// Démarrage du serveur
app.listen(port, "0.0.0.0", async () => {
  console.log(`Serveur lancé sur http://localhost:${port}`);

  try {
    await sequelize.authenticate();
    console.log("Connexion à la base de données réussie.");
  } catch (err) {
    console.error("Erreur de connexion à la base de données :", err);
  }

  await initDb(); // Synchronisation de la base de données
});

// Middleware 404
app.use((req, res) => {
  res.status(404).json({ message: "Route non trouvée." });
});

// Middleware global de gestion d’erreurs
app.use((err, req, res, next) => {
  console.error("Erreur serveur :", err);
  res.status(500).json({
    message: "Une erreur interne est survenue.",
    error: err.message,
  });
});
