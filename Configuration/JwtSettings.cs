public class JwtSettings
{
    public required string SecretKey { get; set; }
    public int TokenLifetimeMinutes { get; set; }
}
