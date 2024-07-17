namespace UserAccessor.Models
{
    public class DbSettings
    {
        public string? Host { get; set; }
        
        public int? Port { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? DatabaseName { get; set; }

        public string ConnectionString {
            get {
                return $"server={Host}; userid={Username};pwd={Password};port={Port};database={DatabaseName}";
            }
        }
    }
}
