namespace tablero_bi.Domain.Entities
{
    public class RevokedToken
    {
        public string Token { get; set; }
        public DateTime RevokedAt { get; set; }
        public DateTime Expiration { get; set; }
        public string Message { get; set; }
    }
}
