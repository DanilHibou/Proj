using AD.Areas.Identity.Pages.Account;

namespace AD.Models
{
    public class LogInOutLog
    {
        public int id { get; set; }
        public string? UserName { get; set; } 
        public DateTime Timestamp { get; set; } 
        public string? ActionType { get; set; } 
        public string? UserId { get; set; } 

        public ApplicationUser? User { get; set; }
    }
}
