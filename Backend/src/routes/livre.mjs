import express from "express";
import { sequelize } from "../db/sequelize.mjs";
import { Sequelize, DataTypes, Op } from "sequelize";
import { LivreModel, defineRelations } from "../models/structure.mjs";
import { success } from "./helper.mjs";

const livreRouter = express.Router();

const Livre = LivreModel(sequelize, DataTypes);
defineRelations();

// Fonction utilitaire pour préparer les données à envoyer au client
const formatLivreForClient = (
  livreInstance,
  includeContent = false,
  fullContent = false
) => {
  const data = livreInstance.toJSON();

  // Convertit l'image en base64 si elle existe
  if (data.imageCouverture) {
    data.imageCouverture = Buffer.from(data.imageCouverture).toString("base64");
  }

  // Gère l'inclusion ou non du contenu du livre
  if (!includeContent) {
    delete data.contenu;
  } else if (!fullContent) {
    data.contenu = data.contenu?.substring(0, 500) + "...";
  }

  return data;
};

// GET /livres : retourne une liste limitée de livres
livreRouter.get("/", async (req, res) => {
  try {
    const books = await Livre.findAll({ limit: 10 });
    if (books.length === 0) {
      return res.status(404).json({ message: "Aucun livre trouvé." });
    }

    const formattedBooks = books.map((book) => formatLivreForClient(book));
    return res.json(
      success("Liste des livres récupérée avec succès.", formattedBooks)
    );
  } catch (error) {
    console.error("Erreur lors de la récupération des livres :", error);
    return res.status(500).json({
      message: "Impossible de récupérer les livres.",
      data: error.message,
    });
  }
});

// GET /livres/search : recherche par titre, auteur ou contenu
livreRouter.get("/search", async (req, res) => {
  const { query, order } = req.query;

  if (!query) {
    return res
      .status(400)
      .json({ message: "Le paramètre 'query' est requis." });
  }

  const sortOrder = order === "asc" ? "ASC" : "DESC";

  try {
    const livres = await Livre.findAll({
      where: {
        [Op.or]: [
          { titre: { [Op.like]: `%${query}%` } },
          { auteur: { [Op.like]: `%${query}%` } },
          { contenu: { [Op.like]: `%${query}%` } },
        ],
      },
      order: [["createdAt", sortOrder]],
    });

    if (livres.length === 0) {
      return res
        .status(404)
        .json({ message: "Aucun résultat pour la recherche." });
    }

    const formattedLivres = livres.map((book) => formatLivreForClient(book));
    return res.json(success("Résultats de la recherche.", formattedLivres));
  } catch (error) {
    console.error("Erreur recherche livres :", error);
    return res.status(500).json({
      message: "Erreur lors de la recherche.",
      data: error.message,
    });
  }
});

// GET /livres/:id : retourne les détails d’un livre avec contenu complet
livreRouter.get("/:id", async (req, res) => {
  try {
    const id = parseInt(req.params.id, 10);
    if (isNaN(id)) {
      return res.status(400).json({ message: "ID invalide." });
    }

    const book = await Livre.findByPk(id);

    if (!book) {
      return res
        .status(404)
        .json({ message: "Le livre demandé n'existe pas." });
    }

    const formattedBook = formatLivreForClient(book, true, true);
    return res.json(
      success(`Livre ID ${id} récupéré avec succès.`, formattedBook)
    );
  } catch (error) {
    console.error("Erreur récupération livre :", error);
    return res.status(500).json({
      message: "Erreur lors de la récupération du livre.",
      data: error.message,
    });
  }
});

export { livreRouter };
