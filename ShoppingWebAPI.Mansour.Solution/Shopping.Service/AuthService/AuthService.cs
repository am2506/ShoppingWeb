using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Shopping.Core.IdentityModels;
using Shopping.Core.IServices;

namespace Shopping.Service.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }



        public async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            /// To build or Creat Token Need Three Parts
            /// Header 
            /// Payload
            /// Verify Signature

            //=============Payload===============
            #region Claims - Payload - Information Exchange
            //build Private Claims(User-Defined)
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim (ClaimTypes.Name,user.DisplayName)
            };
            // If want to get user Roles (Optional)
            var UserRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in UserRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            #endregion

            //===================Signature============
            //build SecurityKey
            var authkeybytes = Encoding.UTF8.GetBytes(_configuration["JWT:AuthKey"]);
            var authkey = new SymmetricSecurityKey(authkeybytes);

            //====Register Claims=====
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationByDays"] ?? "0")),
                claims: authClaims,// --> payload
                signingCredentials:new SigningCredentials(authkey,SecurityAlgorithms.HmacSha256Signature)
                );

            //Handle Your Token 
            return new JwtSecurityTokenHandler().WriteToken(token);
           
        }
    }
}
