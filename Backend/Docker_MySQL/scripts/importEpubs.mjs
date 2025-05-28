import fs from "fs/promises";
import path from "path";
import Epub from "epub";
import { v4 as uuidv4 } from "uuid";
import { decode } from "html-entities";
import { sequelize, Livre } from "../../src/db/sequelize.mjs";

// Dossier où sont stockés les fichiers EPUB à importer
const epubFolder = path.resolve("../../uploads/epubs");
await fs.mkdir(epubFolder, { recursive: true }); // Crée le dossier s'il n'existe pas

export const extractAndInsert = async () => {
  const files = await fs.readdir(epubFolder);

  // Réinitialise la table des livres
  await Livre.destroy({ where: {} });
  console.log("Tous les livres existants ont été supprimés.");

  let count = 0;

  for (const file of files) {
    if (!file.endsWith(".epub")) continue;

    const filePath = path.join(epubFolder, file);

    await new Promise((resolve) => {
      const epub = new Epub(filePath);

      epub.on("end", async () => {
        const titre = epub.metadata?.title || file.replace(".epub", "");
        const auteur = epub.metadata?.creator || "Auteur inconnu";
        const langue = epub.metadata?.language || "inconnu";

        // Sujet : peut être une chaîne ou un tableau
        const sujet = Array.isArray(epub.metadata?.subject)
          ? epub.metadata.subject.join(", ")
          : epub.metadata?.subject || null;

        // Génère un UUID fiable
        let rawId = epub.metadata?.identifier;
        if (Array.isArray(rawId)) rawId = rawId[0];
        const uuid =
          typeof rawId === "string" && rawId.trim() ? rawId.trim() : uuidv4();

        const dateStr = epub.metadata?.date;
        const date_modification = dateStr ? new Date(dateStr) : null;

        let imageBuffer = null;
        const spineIds = epub.flow.map((item) => item.id);
        let contenu = "";

        // Extrait et nettoie le texte de tous les chapitres du livre
        const fetchAllText = async () => {
          for (const id of spineIds) {
            try {
              const text = await new Promise((resolveText) => {
                epub.getChapter(id, (err, text) => {
                  if (err) return resolveText("");
                  let cleaned = decode(text.replace(/<\/?[^>]+(>|$)/g, " "));
                  cleaned = cleaned.replace(/\s+/g, " ").trim();
                  resolveText(cleaned);
                });
              });
              contenu += text + "\n\n";
            } catch {
              console.warn(`Erreur lors de la lecture du chapitre ${id}`);
            }
          }

          // Supprime les sauts de ligne multiples
          contenu = contenu
            .replace(/(\n\s*){2,}/g, "\n\n")
            .replace(/\n{3,}/g, "\n\n")
            .trim();
        };

        const insertLivre = async () => {
          try {
            // Évite les doublons en vérifiant l'UUID
            const exists = await Livre.findOne({ where: { uuid } });
            if (exists) {
              console.log(`Livre "${titre}" déjà importé, ignoré.`);
              return resolve();
            }

            await fetchAllText();

            await Livre.create({
              uuid,
              titre,
              auteur,
              langue,
              sujet,
              date_modification,
              imageCouverture: imageBuffer,
              contenu,
              nbPage: 100,
              createdAt: new Date(),
            });

            console.log(`Livre importé : ${titre}`);
            count++;
            resolve();
          } catch (error) {
            console.error(`Erreur sur "${file}" :`, error.message);
            resolve();
          }
        };

        // Recherche d’une image de couverture
        const coverItem = Object.values(epub.manifest).find(
          (item) =>
            item.href?.toLowerCase().includes("cover") &&
            item["media-type"]?.startsWith("image")
        );

        if (coverItem?.id) {
          epub.getImage(coverItem.id, async (err, imageData) => {
            if (!err && imageData) {
              imageBuffer = imageData;
            }
            await insertLivre();
          });
        } else {
          await insertLivre();
        }
      });

      epub.on("error", (err) => {
        console.error(`EPUB invalide : ${file}`, err.message);
        resolve();
      });

      epub.parse();
    });
  }

  console.log(`${count} livres importés avec succès.`);
};

// Synchronisation des modèles Sequelize et lancement de l’importation
await sequelize.sync();
await extractAndInsert();
console.log("Importation terminée.");
