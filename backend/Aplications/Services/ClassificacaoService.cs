using RoyalGamess.Aplications.DTOs.ClassificacaoDto;
using RoyalGamess.Aplications.DTOs.GeneroDto;
using RoyalGamess.Domains;
using RoyalGamess.Exceptions;
using RoyalGamess.Repositorys;
using RoyalGamess.Interfaces;

namespace RoyalGamess.Aplications.Services
{
    public class ClassificacaoService
    {

        private readonly IClassificacaoIndicativa _repository;
        public ClassificacaoService(IClassificacaoIndicativa repository)
        {
            _repository = repository;
        }

        private static LerClassificacaoDto LerDto(ClassificacaoIndicativa classificacaoI)
        {
            LerClassificacaoDto lerClassificacaoDto = new LerClassificacaoDto()
            {
                classificacaoId = classificacaoI.ClassificacaoIndicativaId,
                nomeClassificacao = classificacaoI.Classificacao
            };

            return lerClassificacaoDto;
        }

        public List<LerClassificacaoDto> Listar()
        {
            List<ClassificacaoIndicativa> classificacaoListaDomain = _repository.Listar();
            List<LerClassificacaoDto> classificacaoDto = classificacaoListaDomain.Select(query => LerDto(query)).ToList();

            return classificacaoDto;
        }

        public LerClassificacaoDto ObterPorId(int id)
        {
            ClassificacaoIndicativa? classificacaoDomain = _repository.ObterPorId(id);
            if (classificacaoDomain == null)
                throw new DomainException("Classificacao Indicativa não existe");

            return LerDto(classificacaoDomain);
        }



        public LerClassificacaoDto Adicionar(CriarClassificacaoDto criarDto)
        {
            if (_repository.ClassificacaoExiste(criarDto.nomeClassificacao))
            {
                throw new DomainException("Classificação já existe ");
            }

            ClassificacaoIndicativa classificacao = new ClassificacaoIndicativa
            {
                Classificacao = criarDto.nomeClassificacao
            };

            _repository.Adicionar(classificacao);
            return LerDto(classificacao);

        }


        public LerClassificacaoDto Atualizar(int id, CriarClassificacaoDto criarDto)
        {
            ClassificacaoIndicativa classificacaoBanco = _repository.ObterPorId(id);
            if (classificacaoBanco == null)
                throw new DomainException("Classificação não existe");

            classificacaoBanco.Classificacao = criarDto.nomeClassificacao;
            _repository.Atualizar(classificacaoBanco);
            return LerDto(classificacaoBanco);

        }

        public void Remover(int id)
        {
            ClassificacaoIndicativa classificacaoBanco = _repository.ObterPorId(id);
            if (classificacaoBanco == null)
                throw new DomainException("Classificação não existe");

            _repository.Remover(id);

        }
    }
}
