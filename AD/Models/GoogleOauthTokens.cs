using AD.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Identity;

namespace AD.Models
{
    public class GoogleOauthTokens
    {
        public int id { get; set; }
        public string? refreshToken { get; set; }

       
        public DateTime? RefreshTokenIssued { get; set; }
        //public DateTime? AccessTokenIssued { get; set; }
        public string? UserId { get; set; } 

        public ApplicationUser? User { get; set; }

    }
}
