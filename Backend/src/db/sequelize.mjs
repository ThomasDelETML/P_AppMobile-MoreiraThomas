import { Sequelize, DataTypes } from "sequelize";
import { LivreModel, defineRelations } from "../models/structure.mjs";

// Connexion à la base de données MySQL
const sequelize = new Sequelize("db_livre", "root", "root", {
  host: "localhost",
  port: 6033,
  dialect: "mysql",
  logging: false, // Désactive les logs SQL dans la console
});

// Initialisation du modèle Livre
const Livre = LivreModel(sequelize, DataTypes);

// Définition des relations entre les modèles
defineRelations();

// Fonction de synchronisation de la base de données
const initDb = async () => {
  await sequelize.sync({ alter: true });
  console.log("DB synchronisée.");
};

export { sequelize, initDb, Livre };
