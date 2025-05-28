// Modèle Sequelize représentant la table t_livre
const LivreModel = (sequelize, DataTypes) => {
  return sequelize.define(
    "t_livre",
    {
      livre_id: {
        type: DataTypes.INTEGER,
        autoIncrement: true,
        primaryKey: true,
      },
      uuid: {
        type: DataTypes.STRING,
        allowNull: false,
        unique: true, // Identifiant externe unique (EPUB ou généré)
      },
      titre: {
        type: DataTypes.STRING(255),
        allowNull: false,
      },
      auteur: {
        type: DataTypes.STRING,
        allowNull: true,
      },
      langue: {
        type: DataTypes.STRING(5),
        allowNull: true,
      },
      sujet: {
        type: DataTypes.STRING,
        allowNull: true,
      },
      date_modification: {
        type: DataTypes.DATE,
        allowNull: true, // Date extraite des métadonnées EPUB
      },
      imageCouverture: {
        type: DataTypes.BLOB("long"),
        allowNull: true, // Image binaire (base64 encodée à l'affichage)
      },
      contenu: {
        type: DataTypes.TEXT("long"),
        allowNull: true, // Texte complet du livre
      },
      createdAt: {
        type: DataTypes.DATE,
        allowNull: false,
        defaultValue: DataTypes.NOW, // Date d'insertion
      },
    },
    {
      freezeTableName: true, // Empêche Sequelize de renommer la table automatiquement
      tableName: "t_livre",
      timestamps: false,
    }
  );
};

const defineRelations = () => {};

export { LivreModel, defineRelations };
