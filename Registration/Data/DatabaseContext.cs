using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Registration.Configuration.Entity;
using Registration.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Registration.Data
{
    public class DatabaseContext : IdentityDbContext<User> 

    {
       
        public DatabaseContext(DbContextOptions options) : base(options)
        { }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);            
            builder.ApplyConfiguration(new RoleConfiguration());
           // builder.ApplyConfiguration(new UserConfiguration(_configuration));
            builder.ApplyConfiguration(new UserConfiguration());

           


        }       


    }



}
   

