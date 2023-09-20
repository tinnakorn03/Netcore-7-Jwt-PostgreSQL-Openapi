namespace NetcoreJwtJsonbOpenapi.Helpers
{
    public class AppSettings
    {
        public string? Secret { get; set; }
        public ConnectionStrings? ConnectionStrings { get; set; }
    }

    public class ConnectionStrings
    { 
        public string? Server { get; set; }
        public string? Database { get; set; }
        public string? UserId { get; set; }
        public string? Password { get; set; }
        public string? DefaultConnection { get; set; }
    }
}
