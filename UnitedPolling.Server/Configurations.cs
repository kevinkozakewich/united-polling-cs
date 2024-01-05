namespace UnitedPolling
{
    public class Configurations
    {
        public class SendGrid
        {
            public string apiKey { get; set; }
            public string fromEmail { get; set; }
            public string fromName { get; set; }
            public string recipientEmail { get; set; }
            public string recipientName { get; set; }
        }
    }
}
