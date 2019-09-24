namespace GuoGuoCommunity.Domain
{
    public class Token
    {
        //Token
        public string Access_token { get; set; }
        
        //Refresh Token
        public string Refresh_token { get; set; }
        
        //几秒过期
        public int Expires_in { get; set; }
    }
}
