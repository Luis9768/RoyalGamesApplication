using RoyalGamess.Aplications.DTOs.JogoDto;
using RoyalGamess.Domains;

namespace RoyalGamess.Aplications.Conversões
{
    public class JogoParaDTO
    {

        public static LerJogoDto converterParaDTO(Jogo jogo)
        {
            return new LerJogoDto
            {
                JogoId = jogo.JogoId,
                Nome = jogo.Nome,
                Descricao = jogo.Descricao,
                StatusJogo = jogo.StatusJogo ?? true,
                // StatusJogo = jogo.StatusJogo,
                plataformaIds = jogo.PlataformaIdFK.Select(id => id.PlataformaId).ToList(),
                Plataforma = jogo.PlataformaIdFK.Select(nomePlat => nomePlat.Nome).ToList(),
                Genero = jogo.GeneroIdFK.Select(nomeGen => nomeGen.Nome).ToList(),
                generoIds = jogo.GeneroIdFK.Select(id => id.GeneroId).ToList(),
                classificacaoId = jogo.ClassificaçãoIdFK,
                Classificacao = jogo.ClassificaçãoIdFKNavigation?.Classificacao,
                ImagemURL = $"jogo/{jogo.JogoId}/imagem", 

                UsuarioId = jogo.UsuarioIdFK,
                UsuarioEmail = jogo.UsuarioIdFKNavigation?.Email,
                UsuarioNome = jogo.UsuarioIdFKNavigation?.Nome
            };
        }

    }
}
