using RoyalGamess.Exceptions;

namespace RoyalGamess.Aplications.Regras
{
    public class HorarioAlteracaoJogo
    {
        public static void ValidarHorario()
        {
            var agora = DateTime.Now.TimeOfDay;
            var abertura = new TimeSpan(10, 0, 0);
            var fechamento = new TimeSpan(15, 59, 59);

            var estaAberto = agora >= abertura && agora <= fechamento;
            if (estaAberto)
            {
                throw new DomainException("Produto só pode ser alterado fora do horário de funcionamento!");
            }
        }
    }
}