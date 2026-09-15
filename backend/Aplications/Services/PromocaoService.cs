using RoyalGamess.Aplications.DTOs.PromocaoDto;
using RoyalGamess.Domains;
using RoyalGamess.Exceptions;
using RoyalGamess.Interfaces;
using RoyalGamess.Repositorys;

namespace RoyalGamess.Aplications.Services
{
    public class PromocaoService
    {
        private readonly IPromocaoRepository _repository;
        public PromocaoService(IPromocaoRepository repository)
        {
            _repository = repository;
        }
        private static LerPromocaoDto LerDto (Promocao promocao)
        {
            LerPromocaoDto lerPromo = new LerPromocaoDto
            {
               PromocaoId = promocao.PromocaoId,
               DataExpiracao = promocao.DataExpiração,
               Nome = promocao.Nome,
               StatusPromocao = promocao.StatusPromocao
            };
            return lerPromo;
        }
        public List<LerPromocaoDto> Listar()
        {
            List<Promocao> listarPromo = _repository.Listar();
            List<LerPromocaoDto> promoDto = listarPromo.Select(p => LerDto(p)).ToList();
            return promoDto;
        }
        public LerPromocaoDto ObterPorId(int id)
        {
            Promocao? promocao = _repository.ObterPorId(id);
            if(promocao == null)
            {
                throw new DomainException("Promoção não encontrada!");
            }
            return LerDto(promocao);
        }
        private static void ValidarNome(string Nome)
        {
            if (string.IsNullOrEmpty(Nome))
            {
                throw new DomainException("Nome inválido!");
            }
        }
        private static void ValidarDataExpiracao(DateTime dataExpiracao)
        {
            if (dataExpiracao <= DateTime.Now)
            {
                throw new DomainException("Data de expiração deve ser futura!");
            }
        }
        public LerPromocaoDto Adicionar(CriarPromocaoDto criarDto)
        {
            ValidarDataExpiracao(criarDto.DataExpiracao);
            ValidarNome(criarDto.Nome);
            if (_repository.NomeExiste(criarDto.Nome))
            {
                throw new DomainException("Promoção já existe!");
            }
            Promocao promo = new Promocao
            {
                Nome = criarDto.Nome,
                DataExpiração = criarDto.DataExpiracao,
                StatusPromocao = criarDto.StatusPromocao
            };
            _repository.Adicionar(promo);
            return LerDto(promo);
        }
        public LerPromocaoDto Atualizar(int id, CriarPromocaoDto criarDto)
        {
            ValidarNome(criarDto.Nome);
            Promocao? promoBanco = _repository.ObterPorId(id);
            if(promoBanco == null)
            {
                throw new DomainException("Promoção não encontrada!");
            }
            if (_repository.NomeExiste(criarDto.Nome, promocaoAtual: id))
            {
                throw new DomainException("Já existe outra promoção com esse nome!");
            }
            promoBanco.Nome = criarDto.Nome;
            promoBanco.DataExpiração = criarDto.DataExpiracao;
            promoBanco.StatusPromocao = criarDto.StatusPromocao;
            _repository.Atualizar(id,promoBanco);
            return LerDto(promoBanco);
        }
        public void Remover(int id)
        {
            Promocao promocao = _repository.ObterPorId(id);
            if(promocao == null)
            {
                throw new DomainException("Promoção não encontrada!");
            }
            _repository.Remover(id);
        }

    }
}
