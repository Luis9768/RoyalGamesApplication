using RoyalGamess.Aplications.DTOs.PlataformaDto;
using RoyalGamess.Domains;
using RoyalGamess.Exceptions;
using RoyalGamess.Interfaces;
using RoyalGamess.Repositorys;

namespace RoyalGamess.Aplications.Services
{
    public class PlataformaService
    {
        private readonly IPlataformaRepository _repository;
        public PlataformaService(IPlataformaRepository repository)
        {
            _repository = repository;
        }
        private static LerPlataformaDto LerDto(Plataforma plataforma)
        {
            LerPlataformaDto lerPlatDto = new LerPlataformaDto
            {
                Nome = plataforma.Nome,
                PlataformaId = plataforma.PlataformaId
            };
            return lerPlatDto;
        }
        public List<LerPlataformaDto> Listar()
        {
            List<Plataforma> listaPlat = _repository.Listar();
            List<LerPlataformaDto> lista = listaPlat.Select(n => LerDto(n)).ToList();
            return lista;
        }
        public LerPlataformaDto ObterPorId(int id)
        {
            Plataforma? plataforma = _repository.ObterPorId(id);
            if (plataforma == null)
            {
                throw new DomainException("Plataforma não encontrada!");
            }
            return LerDto(plataforma);
        }
        public Plataforma ObterPorNome(string nome)
        {
            Plataforma? plataforma = _repository.ObterPorNome(nome);
            if (plataforma == null)
            {
                throw new DomainException("Plataforma não encontrada!");
            }
            return plataforma;
        }

        private static void ValidarNome(string nome)
        {
            if (string.IsNullOrEmpty(nome))
            {
                throw new DomainException("Nome inválido!");
            }
        }
        public LerPlataformaDto Adicionar(CriarPlataformaDto criarDto)
        {
            ValidarNome(criarDto.Nome);
            if (_repository.NomeExiste(criarDto.Nome))
            {
                throw new DomainException("Plataforma já existente!");
            }
            Plataforma plataforma = new Plataforma
            {
                Nome = criarDto.Nome
            };
            _repository.Adicionar(plataforma);
            return LerDto(plataforma);
        }
        public LerPlataformaDto Atualizar(int id, CriarPlataformaDto criarDto)
        {
            ValidarNome(criarDto.Nome);
            Plataforma? platBanco = _repository.ObterPorId(id);
            if (platBanco == null)
            {
                throw new DomainException("Plataforma não encontrada!");
            }
            if (_repository.NomeExiste(criarDto.Nome, plataformaAtual: id))
            {
                throw new DomainException("Já existe uma plataforma com este nome!");
            }
            platBanco.Nome = criarDto.Nome;
            _repository.Atualizar(id, platBanco);
            return LerDto(platBanco);
        }
        public void Remover(int id)
        {
            Plataforma? platBanco = _repository.ObterPorId(id);
            if (platBanco == null)
            {
                throw new DomainException("Plataforma não encontrada!");
            }
            _repository.Remover(id);
        }


    }
}
