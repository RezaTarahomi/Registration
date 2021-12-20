using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.Services
{
    public static  class RefreshTokenValidator
    {
        public static IConfiguration Configuration;

        public static bool Validate (string refreshToken)
        {
            var key = Configuration.GetSection("JWT").GetSection("RefreshTokenSecret").Value;
            var validIssuer = Configuration.GetSection("JWT").GetSection("Issuer").Value;

            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = validIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            };


            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            try
            {
            tokenHandler.ValidateToken(refreshToken,validationParameters,out SecurityToken validatedToken);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
