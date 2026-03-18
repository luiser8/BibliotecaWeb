namespace Domain.Ports
{
    public interface ICodigoRecuperacion
    {
        Task<string> StringCodigoAsync();
    }
}
