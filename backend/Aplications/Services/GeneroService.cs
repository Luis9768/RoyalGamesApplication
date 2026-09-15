using RoyalGamess.Aplications.DTOs.GeneroDto;
using RoyalGamess.Domains;
using RoyalGamess.Exceptions;
using RoyalGamess.Repositorys;
using RoyalGamess.Interfaces;

namespace RoyalGamess.Aplications.Services
{
    public class GeneroService
    {
        private readonly IGeneroRepository _repository;

        public GeneroService(IGeneroRepository repository)
        {
            _repository = repository;
        }

        private static LerGeneroDto LerDto(Genero genero)
        {
            LerGeneroDto lerGeneroDto = new LerGeneroDto
            {
                GeneroId = genero.GeneroId,
                Nome = genero.Nome
            };

            return lerGeneroDto;
        }

        public List<LerGeneroDto> Listar()
        {
            List<Genero> generoLista = _repository.Listar();
            List<LerGeneroDto> lerGenero = generoLista.Select(GeneroP => LerDto(GeneroP)).ToList();

            return lerGenero;
        }

        public LerGeneroDto ObterPorId(int id)
        {
            Genero? genero = _repository.ObterPorId(id);
            if (genero == null)
            {
                throw new DomainException("Genero não existe!");
            }

            return LerDto(genero);
        }

        public LerGeneroDto ObterPorNome(string nomeGenero)
        {
            Genero? genero = _repository.ObterPorNome(nomeGenero);
            if (genero == null)
            {
                throw new DomainException("Gênero não possui nome/não existe");
            }

            return LerDto(genero);
        }


        public LerGeneroDto Adicionar(CriarGeneroDto criarDtoGenero)
        {
            if (_repository.NomeGeneroExiste(criarDtoGenero.Nome))
            {
                throw new DomainException("Não existe gênero sem nome");
            }

            Genero genero = new Genero
            {
                Nome = criarDtoGenero.Nome
            };

            _repository.Adicionar(genero);
            return LerDto(genero);

        }

        public LerGeneroDto Atualizar(int id, CriarGeneroDto criarDtoGenero)
        {
            Genero generoBanco = _repository.ObterPorId(id);
            if (generoBanco == null)
            {
                throw new DomainException("Gênero não encontrado");
            }

            Genero generoDto = _repository.ObterPorNome(criarDtoGenero.Nome);
            if (generoDto != null && generoDto.GeneroId != id)
            {
                throw new DomainException("Gênero não existente");
            }

            generoBanco.Nome = criarDtoGenero.Nome;
            _repository.Atualizar(generoBanco);
            return LerDto(generoBanco);
        }

        public void Remover(int id)
        {
            Genero? genero = _repository.ObterPorId(id);
            if (genero == null)
            {
                throw new DomainException("Genero nao existe");
            }
            _repository.Remover(id);
        }

    }

}
