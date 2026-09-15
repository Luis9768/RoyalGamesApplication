using Microsoft.IdentityModel.Tokens;
using RoyalGamess.Domains;
using RoyalGamess.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RoyalGamess.Aplications.Autenticacao
{
    public class GeradorTokenJWT
    {
        private readonly IConfiguration _config;
        public GeradorTokenJWT(IConfiguration configuration)
        {
            _config = configuration;
        }
        public string GerarToken(Usuario usuario)
        {
            if (usuario.StatusUsuario == false)
            {
                throw new DomainException("Usuario Inátivo. Você não pode realizar alterações!");
            }
            //kjey usada para assinar o token e garantir que ele não seja alterado por terceiros
            var chave = _config["Jwt:Key"];
            //quem gerou o token (nome da API/ sistema que gerou o token)
            //API valida se o token veio do emissor correto
            var issuer = _config["Jwt:Issuer"];
            // para quem o token foi criado
            //define qual sistema pode usar o token para acessar os recursos protegidos
            var audience = _config["Jwt:Audience"];
            //Tempo de expiração do token, em minutos
            var expiraEmMinutos = int.Parse(_config["Jwt:ExpireEmMinutos"]!);
            //Converte para bytes
            var keyBytes = Encoding.UTF8.GetBytes(chave);

            if (keyBytes.Length < 32)
            {
                throw new DomainException("JWT: Key precisa ter pelo menos 32 caracteres (256 bits).");
            }
            //Cria a chave de segurança usada para assinar o token
            var securityKey = new SymmetricSecurityKey(keyBytes);

            //Cria as credenciais de assinatura usando a chave de segurança e o algoritmo de hash
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Cria as claims (informações) que serão incluídas no token
            //essas informações podem ser recuperadas pela API para identificar quem esta logado e quais permissões ele tem
            var claims = new List<Claim>
            {
                //id do usuário para saber quem fez a ação
                new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),

                //nome do usuario
                new Claim(ClaimTypes.Name, usuario.Nome),

                //email do usuario
                new Claim(ClaimTypes.Email, usuario.Email)
            };
            //cria o token JWT com as informações fornecidas
            var token = new JwtSecurityToken(
                issuer: issuer,     //quem emitiu o token
                audience: audience, //para quem o token foi criado
                claims: claims,     //informações sobre o usuário e suas permissões
                expires: DateTime.Now.AddMinutes(expiraEmMinutos), //tempo de expiração do token
                signingCredentials: credentials //assinatura de segurança para garantir a integridade do token
            );

            //converte o token para uma string compacta que pode ser enviada ao cliente
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
