namespace AuthApi.Services
{
    public interface IEmailService
    {
        /// <summary>
        /// Send a verifcation email to confirmed user email 
        /// </summary>
        /// <param name="email">email of the user to confirm</param>
        /// <param name="token">confirmation token</param>
        /// <returns></returns>
        Task SendVerificationEmail(string email, string token);
    }
}