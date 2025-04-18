namespace JWTIdentityApi.JwtConfig
{
    public class JwtTokenOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public int ExpiresIn { get; set; }
    }
}
