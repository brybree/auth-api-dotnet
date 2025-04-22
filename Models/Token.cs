namespace AuthApi.Models
{
    /// <summary>
    /// Class <c>Token</c> represent a token.
    /// </summary>
    public class Token
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}