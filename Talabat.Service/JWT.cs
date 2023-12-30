namespace Talabat.Service
{
    public class JWT
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double ExpiresOn { get; set; }
    }
}
