// Structure standardisée pour les réponses API avec message et données
const success = (message, data) => {
  return {
    message: message,
    data: data,
  };
};

export { success };
