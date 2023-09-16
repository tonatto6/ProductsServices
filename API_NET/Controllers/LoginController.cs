using API_NET.DTO;
using API_NET.Models;
using API_NET.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API_NET.Controllers
{
    [Route("Login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<LoginController> log;
        private readonly IUserApiServices services;

        public LoginController(IConfiguration config, ILogger<LoginController> l, IUserApiServices userServices)
        {
            configuration = config;
            this.log = l;
            services = userServices;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<UserApiDTO> Login(LoginApi usuarioLogin)
        {
            UserApi usuario = null;
            usuario = AutenticateUser(usuarioLogin);
            if (usuario == null)
            {
                throw new Exception("Credenciales no validas");
            }
            else
            {
                // Generar token
                GenerateTokenJWT(usuario);
            }

            return usuario.transformToUserApiDTO();
        }

        private UserApi AutenticateUser(LoginApi usuarioLogin)
        {
            UserApi usuario = services.UserSeek(usuarioLogin);
            return usuario;
        }
    
        private UserApi GenerateTokenJWT(UserApi usuarioInfo)
        {
            // Clave secreta
            var _symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));

            // Declaramos el algoritmo a utilizar
            var _signingCredentials = new SigningCredentials(
                    _symmetricSecurityKey, SecurityAlgorithms.HmacSha256
                );
            // Crea la cabecera
            var _header = new JwtHeader(_signingCredentials);

            var _claims = new[] {
                new Claim("Usuario", usuarioInfo.User),
                new Claim("Email", usuarioInfo.EmailUser),
                new Claim(JwtRegisteredClaimNames.Email, usuarioInfo.EmailUser)
            };

            // Payload
            var _payload = new JwtPayload(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: _claims,
                 notBefore: DateTime.UtcNow,
                     expires: DateTime.UtcNow.AddHours(1)
                );

            // Token
            var _token = new JwtSecurityToken(_header, _payload);
            usuarioInfo.Token = new JwtSecurityTokenHandler().WriteToken(_token);
            return usuarioInfo;
        }
    }
}
