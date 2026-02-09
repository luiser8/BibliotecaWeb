using Domain.Ports;

namespace Infrastructure.Security
{
    public class CodigoRecuperacion : ICodigoRecuperacion
    {
        public string StringCodigoAsync()
        {
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            return new string([.. Enumerable.Repeat(caracteres, 6).Select(s => s[Random.Shared.Next(s.Length)])]);
        }
    }
}
