using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web.Http;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
    public class UserController : ApiController
    {
        UserEntities db = new UserEntities();
        object data = new { };

        [System.Web.Http.Route("api/Register")]
        [HttpPost]
        public IHttpActionResult Register(users user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var uniqueUser = db.users.Where(u => u.email == user.email).FirstOrDefault();

            if (uniqueUser != null)
            {
                data = new
                {
                    success = false,
                    message = "The email already exist!"
                };
                return Content(HttpStatusCode.InternalServerError, data);
            }
            if (user.password.Equals(user.confirmPassword))
            {
                var password = PasswordHash.HashPassword(user.password);

                user.password = password;
                user.confirmPassword = password;

                db.users.Add(user);
                db.SaveChanges();

                data = new
                {
                    success = true,
                    message = "Successfully registered user"
                };

                return Ok(data);
            }
            else return BadRequest();
        }

        [System.Web.Http.Route("api/Login")]
        [HttpPost]
        public IHttpActionResult Login([FromBody] users login)
        {
            var user = db.users.Where(u => u.email.Equals(login.email)).FirstOrDefault();

            if (user == null)
            {
                data = new
                {
                    success = false,
                    message = "Cannot find user with this credentials!"
                };

                return Content(HttpStatusCode.InternalServerError, data);
            }

            if (PasswordHash.VerifyHashedPassword(user.password, login.password))
            {
                object token = CreateToken(user.name, 2);
               
                return Ok(token);
            } else
            {
                data = new
                {
                    success = false,
                    message = "Password is incorrect!"
                };

                return Content(HttpStatusCode.InternalServerError, data);
            }
        }

        private object CreateToken(string username, int hours)
        {
            object data = new { };

            DateTime issuedAt = DateTime.UtcNow;
            DateTime expires = DateTime.UtcNow.AddHours(hours);

            var tokenHandler = new JwtSecurityTokenHandler();

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username)
            });

            const string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
            var now = DateTime.UtcNow;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);

            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(
                        issuer: "http://localhost:44391",
                        audience: "http://localhost:44391",
                        subject: claimsIdentity,
                        notBefore: issuedAt,
                        expires: expires,
                        signingCredentials: signingCredentials
                        );
            var tokenString = tokenHandler.WriteToken(token);

            data = new
            {
                token = tokenString,
                expires = expires
            };
            return data;
        }
    }
}