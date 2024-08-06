namespace BW_VI___Team_1.Interfaces
{
    /// <summary>
    /// Interfaccia per il servizio delle immagini
    /// </summary>
    public interface IImageSvc
    {
        Task<string> SaveImageAsync(IFormFile imageFile);
    }
}