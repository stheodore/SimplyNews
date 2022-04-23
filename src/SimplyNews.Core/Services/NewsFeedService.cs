using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimplyNews.Core.Models;
using System.Runtime.InteropServices;

namespace SimplyNews.Core.Services
{
    public class NewsFeedService : INewsFeedService
    {
        private readonly string newsFeedBaseUrl = "https://inshortsapi.vercel.app/news?category=";
        private string newsFeedUrl;
        private string fontSize = "2em";

        public async Task<string> GetNewsFeed(string category)
        {
            newsFeedUrl = newsFeedBaseUrl + category;
            var rawContent = await GetNewsContentAsync();
            if (rawContent == null)
                return string.Empty;

            bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            if (isLinux)
                fontSize = "1em";

            NewsRootobject newsObject = JsonConvert.DeserializeObject<NewsRootobject>(rawContent);
            var htmlContent = new StringBuilder();
            string title, description, readmoreUrl;

            htmlContent.Append("<html><head><style>body { font-size: " + fontSize + "; }</style></head><body>");
            if (newsObject.data != null)
            {
                foreach (NewsItem newsitem in newsObject.data)
                {
                    title = newsitem.Title;
                    description = newsitem.Content;
                    readmoreUrl = newsitem.ReadMoreUrl;

                    htmlContent.Append("<a href=\"" + readmoreUrl + "\">" + title + "</a><br/>" + description + "<br/><br/>");
                }
            }

            htmlContent.Append("</body></html>");
            return htmlContent.ToString();
        }

        private async Task<string> GetNewsContentAsync()
        {
            try
            {
                var client = new HttpClient();
                var content = await client.GetStringAsync(newsFeedUrl);
                return content;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }
    }
}
