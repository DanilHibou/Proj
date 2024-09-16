using Microsoft.AspNetCore.Mvc.Formatters;

namespace AD.Models
{
    public class DomainController
    {        
            public int Id { get; set; }
            public string? DomainControllerAddress { get; set; }
            public string? DomainName { get; set; }
            //public string Username { get; set; }
            //public string Password { get; set; }
        
    }
}
