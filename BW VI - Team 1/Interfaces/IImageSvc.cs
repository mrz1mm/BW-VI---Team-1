namespace BW_VI___Team_1.Interfaces
{
    /// <summary>
    /// Interfaccia per il servizio delle immagini
    /// </summary>
    public interface IImageSvc
    {
        /// <summary>
        /// Salva un'immagine
        /// </summary>
        /// <param name="imageFile">File immagine</param>
        /// <returns>URL dell'immagine salvata</returns>
        Task<string> SaveImageAsync(IFormFile imageFile);

        /// <summary>
        /// Elimina un'immagine
        /// </summary>
        /// <param name="imageUrl">URL dell'immagine da eliminare</param>
        /// <returns>Task completato</returns>
        Task DeleteImageAsync(string imageUrl);
    }
}