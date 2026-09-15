namespace RoyalGamess.Aplications.DTOs.LogAlteracaoJogoDto
{
    public class LerLogAlteracaoJogoDto
    {
        public int LogId { get; set; }
        public int? JogoId { get; set; }
        public string NomeAnterior { get; set; } = null!;
        public decimal? PrecoAnterior { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}
