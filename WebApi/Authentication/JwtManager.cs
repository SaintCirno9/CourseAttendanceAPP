using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using CommonShared.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Authentication
{
    public class JwtManager
    {
        private readonly byte[] tokenKey;
        private readonly JwtSecurityTokenHandler tokenHandler;

        public JwtManager(string key)
        {
            tokenKey = Encoding.ASCII.GetBytes(key);
            tokenHandler = new JwtSecurityTokenHandler();
        }

        public string Authenticate(object user)
        {
            var claimIdentity = new ClaimsIdentity();
            if (user is Teacher teacher)
            {
                claimIdentity.AddClaims(new[]
                {
                    new Claim(ClaimTypes.Name, teacher.Id),
                    new Claim(ClaimTypes.Role, "teacher"),
                });
            }
            else if (user is Student student)
            {
                claimIdentity.AddClaims(new[]
                {
                    new Claim(ClaimTypes.Name, student.Id),
                    new Claim(ClaimTypes.Role, "student"),
                });
            }
            else
            {
                return null;
            }

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claimIdentity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GetToken(HttpRequest request)
        {
            return request.Headers.ContainsKey("Authorization")
                ? request.Headers["Authorization"].ToString().Split(" ").Last()
                : null;
        }

        private string DecodeToken(string token)
        {
            if (token == null)
            {
                return null;
            }

            var id =
                (tokenHandler.ReadToken(token) as JwtSecurityToken)?.Claims?.FirstOrDefault()?.Value;
            return id;
        }

        public string GetId(HttpRequest request)
        {
            return DecodeToken(GetToken(request));
        }
    }
}