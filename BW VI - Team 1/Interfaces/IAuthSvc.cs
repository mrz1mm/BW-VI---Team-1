using BW_VI___Team_1.Models.DTO;

namespace BW_VI___Team_1.Interfaces
{
    /// <summary>
    /// Interfaccia per il servizio di autenticazione
    /// </summary>
    public interface IAuthSvc
    {
        /// <summary>
        /// Registra un nuovo utente
        /// </summary>
        /// <param name="model">Dati di registrazione</param>
        /// <returns>True se la registrazione ha successo, altrimenti false</returns>
        Task<bool> Register(RegisterDTO model);

        /// <summary>
        /// Effettua il login di un utente
        /// </summary>
        /// <param name="model">Dati di login</param>
        /// <returns>True se il login ha successo, altrimenti false</returns>
        Task<bool> Login(LoginDTO model);

        /// <summary>
        /// Effettua il logout dell'utente corrente
        /// </summary>
        /// <returns>Task completato</returns>
        Task Logout();

        /// <summary>
        /// Restituisce l'ID dell'utente corrente
        /// </summary>
        /// <returns>ID dell'utente corrente</returns>
        string GetCurrentUserId();
    }
}
