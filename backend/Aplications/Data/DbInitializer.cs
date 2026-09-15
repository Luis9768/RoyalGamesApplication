using System.Security.Cryptography;
using System.Text;
using RoyalGamess.Contexts;
using RoyalGamess.Domains;

namespace RoyalGamess.Aplications.Data
{
    public static class DbInitializer
    {
        public static void Initialize(Royal_GamesssContext context)
        {
            // Garante que o schema/tabelas existam no PostgreSQL
            context.Database.EnsureCreated();

            // 1. Semear Usuário Admin se não houver usuários
            if (!context.Usuario.Any())
            {
                using var sha256 = SHA256.Create();
                var senhaHash = sha256.ComputeHash(Encoding.UTF8.GetBytes("admin@123"));

                var admin = new Usuario
                {
                    Nome = "Carlos Lima (Admin)",
                    Email = "carlos@vhburguer.com",
                    Senha = senhaHash,
                    StatusUsuario = true
                };

                context.Usuario.Add(admin);
                context.SaveChanges();
            }

            // 2. Semear Plataformas
            if (!context.Plataforma.Any())
            {
                context.Plataforma.AddRange(
                    new Plataforma { Nome = "PC" },
                    new Plataforma { Nome = "PlayStation 4" },
                    new Plataforma { Nome = "PlayStation 5" },
                    new Plataforma { Nome = "Xbox One" },
                    new Plataforma { Nome = "Xbox Series X/S" },
                    new Plataforma { Nome = "Nintendo Switch" }
                );
                context.SaveChanges();
            }

            // 3. Semear Gêneros
            if (!context.Genero.Any())
            {
                context.Genero.AddRange(
                    new Genero { Nome = "Terror" },
                    new Genero { Nome = "Sandbox" },
                    new Genero { Nome = "FPS" },
                    new Genero { Nome = "Hack and Slash" },
                    new Genero { Nome = "Soulslike" },
                    new Genero { Nome = "RPG" },
                    new Genero { Nome = "Ação" },
                    new Genero { Nome = "Aventura" }
                );
                context.SaveChanges();
            }

            // 4. Semear Classificação Indicativa
            if (!context.ClassificacaoIndicativa.Any())
            {
                context.ClassificacaoIndicativa.AddRange(
                    new ClassificacaoIndicativa { Classificacao = "Livre" },
                    new ClassificacaoIndicativa { Classificacao = "10 anos" },
                    new ClassificacaoIndicativa { Classificacao = "12 anos" },
                    new ClassificacaoIndicativa { Classificacao = "14 anos" },
                    new ClassificacaoIndicativa { Classificacao = "16 anos" },
                    new ClassificacaoIndicativa { Classificacao = "18 anos" }
                );
                context.SaveChanges();
            }
        }
    }
}
