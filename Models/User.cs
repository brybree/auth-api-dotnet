namespace AuthApi.Models
{
    /// <summary>
    /// Class <c>User</c> represent an user in database.
    /// </summary>
    public class User
    {
        public required string Id { get; set; }
        public required string UserName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpirationTime { get; set; }
    }
}