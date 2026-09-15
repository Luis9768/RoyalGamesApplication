namespace RoyalGamess.Aplications.DTOs.JogoDto
{
    public class CriarJogoDto
    {

        public string Nome { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public IFormFile Imagem { get; set; } = null!;
        public decimal Preco { get; set; }
        public bool StatusJogo { get; set; }
        public List<int> plataformaIds { get; set; } = new();
        public List<int> generoIds { get; set; } = new();

        public int classificacaoId { get; set; }


    }
}
