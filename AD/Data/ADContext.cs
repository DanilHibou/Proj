using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AD.Models;

namespace AD.Data
{
    public class ADContext : DbContext
    {
        public ADContext(DbContextOptions<ADContext> options) : base(options)
        {
        }
        
        public DbSet<ADDomain> DomainController { get; set; }    
        public DbSet<Location> Location { get; set; }
        //public DbSet<WorkerPosition> workerPosition { get; set; }
        public DbSet<UserAccountNames> UserAccountNames { get; set; }
        public DbSet<GoogleDomain> GoogleDomain { get; set; }
        public DbSet<ADOU> ADOU { get; set; }
        public DbSet<ADGroups> ADGroups { get; set; }
        public DbSet<GoogleOU> GoogleOU { get; set; }
        public DbSet<AllowedUsers> AllowedUsers { get; set; }
        public DbSet<GoogleGroups> GoogleGroups { get; set; }
        //public DbSet<WorkerSubdivision> workerSubdivisions { get; set; }
        public DbSet<OrganizationalDivisions> organizationalDivisions { get; set; }

    }
}
