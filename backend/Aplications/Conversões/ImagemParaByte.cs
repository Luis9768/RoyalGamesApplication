using RoyalGamess.Domains;
namespace RoyalGamess.Aplications.Conversões
{
    public class ImagemParaByte
    {

        public static byte[] ConverterImagem(IFormFile Imagem)
        {
            using var ms = new MemoryStream();
            Imagem.CopyTo(ms);
            return ms.ToArray();
        }

    }
}
