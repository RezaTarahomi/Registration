using Registration.Data;
using Registration.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Registration.Services
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginUserDTO userDTO); 
        Task<string> CreateToken(User user);
        string CreateRefreshToken();
    }
}
