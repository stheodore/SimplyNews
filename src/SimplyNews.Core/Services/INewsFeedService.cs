using System.Threading.Tasks;

namespace SimplyNews.Core.Services
{
    public interface INewsFeedService
    {
        Task<string> GetNewsFeed(string categorys);
    }
}
