using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Registration.Model
{   
        public class LoginUserDTO
        {
            [Required]           
            public string UserName { get; set; }

            [Required]
            [StringLength(15, ErrorMessage = "Your Password is limited to {2} to {1} characters", MinimumLength = 6)]
            public string Password { get; set; }
        }
    public class UserDTO
    {
        public string Id { get; set; }       
        public string UserName { get; set; }        
        
        public string Password { get; set; }
       
        public string ConfirmPassword { get; set; }
      
        public string FirstName { get; set; }  
      
        public string LastName { get; set; }
        
        public string FatherName { get; set; }
        
        public string Code { get; set; }  
        public string PhoneNumber { get; set; }
        public string CreatorUserName { get; set; }

    }   

    public class ApiUser : UserDTO
    {
        public string Image { get; set; }
        public string Role { get; set; }       
    }
      

}
