using System.Threading.Tasks;

namespace WebApplication1
{
    public interface IBackgroundJobService
    {
        Task ProcessAsync(string data);
    }
}
