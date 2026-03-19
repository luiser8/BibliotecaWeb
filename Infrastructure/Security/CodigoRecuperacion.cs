using Domain.Ports;

namespace Infrastructure.Security
{
    public class CodigoRecuperacion : ICodigoRecuperacion
    {
        public Task<string> StringCodigoAsync()
        {
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var codigo = new string([.. Enumerable.Repeat(caracteres, 6).Select(s => s[Random.Shared.Next(s.Length)])]);
            return Task.FromResult(codigo);
        }
    }
}
