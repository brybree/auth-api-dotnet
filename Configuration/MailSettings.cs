namespace AuthAPI.Configuration
{
    /// <summary>
    /// Setting class for the mail service
    /// </summary>
    public class MailSettings
    {
        /// <summary>
        /// User mail
        /// </summary>
        public string User { get; } = Environment.GetEnvironmentVariable("MAIL_USER") ?? "";
        /// <summary>
        /// User display name
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// User password
        /// </summary>
        public string Password { get; } = Environment.GetEnvironmentVariable("MAIL_PASSWORD") ?? "";
        /// <summary>
        /// Mail service host
        /// </summary>
        public string Host { get; } = Environment.GetEnvironmentVariable("MAIL_HOST") ?? "";
        /// <summary>
        /// Mail Sender
        /// </summary>
        public string Sender { get; set; }
        /// <summary>
        /// Port used by mail service
        /// </summary>
        public int Port { get; set; }
    }
}