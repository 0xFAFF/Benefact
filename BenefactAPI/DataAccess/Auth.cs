﻿using BenefactAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BenefactAPI.DataAccess
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class AuthRequiredAttribute : Attribute { }

    public class Auth
    {
        public static AsyncLocal<UserData> CurrentUser = new AsyncLocal<UserData>();
        static readonly byte[] key = Convert.FromBase64String("ufbSRUHVCGWsWa1Ny+7oS8Wj9BB2n8m+DqBnLz8PreKH+ykeStpNLo621d3NnvzJRNJjY5yMPTlTkFpZzmmtpg==");
        public static string GenerateToken(UserData user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("name", user.Name),
                    new Claim("id", user.Id.ToString(), ClaimValueTypes.Integer64),
                }),
                Expires = DateTime.UtcNow.AddDays(28),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }
        public static string ValidateUserEmail(string token) => ValidateToken(token)?.FindFirstValue(ClaimTypes.Email);
        public static ClaimsPrincipal ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return principal;
            }
            catch
            {
                return null;
            }
        }
        public static async Task AuthorizeUser(HttpRequest request, IServiceProvider provider)
        {
            CurrentUser.Value = null;
            if (request.Headers.ContainsKey("Authorization"))
            {
                var bearer = request.Headers["Authorization"].FirstOrDefault(h => h.Substring(0, 6) == "Bearer");
                if (bearer != null && bearer.Length > 7)
                {
                    var token = bearer.Substring(7, bearer.Length - 7);
                    var email = ValidateUserEmail(token);
                    if (email != null)
                        CurrentUser.Value = await provider.DoWithDB(async db => await db.Users.FirstOrDefaultAsync(u => u.Email == email));
                }
            }
            if (CurrentUser.Value == null)
                throw new HTTPError("Unauthorized", 401);
        }
    }
}
