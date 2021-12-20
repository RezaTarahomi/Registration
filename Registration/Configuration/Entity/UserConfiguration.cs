using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Registration.Data;
using Registration.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Registration.Configuration.Entity
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
      

        public void Configure(EntityTypeBuilder<User> builder)
        {

           

        }

        
       

      

       
    }


    
}
