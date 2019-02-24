using System.Threading.Tasks;

namespace SeaStore.Services.Interfaces
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
