using AD.Areas.Identity.Pages.Account;

namespace AD.Models
{
    public class YandexOauthTokens
    {
        public int id { get; set; }
        public string? refreshToken { get; set; }

       
        public DateTime? RefreshTokenIssued { get; set; }
        
        public string? UserId { get; set; } // Поле для внешнего ключа

      
        public ApplicationUser User { get; set; }
    }
}
