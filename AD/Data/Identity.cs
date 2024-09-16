using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AD.Models;
using AD.Areas.Identity.Pages.Account;

namespace AD.Data
{
    public class Identity : IdentityDbContext<ApplicationUser>
    {
        
            public Identity(DbContextOptions<Identity> options)
                : base(options)
            {
            }
        public DbSet<GoogleOauthTokens> GoogleOauthTokens { get; set; }
        public DbSet<YandexOauthTokens> YandexOauthTokens { get; set; }
        public DbSet<LogInOutLog> logInOutLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
       
    }

        

    }
}
