using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimplyNews.Core.Models;

namespace SimplyNews.Core.Services
{
    public class NewsFeedService : INewsFeedService
    {
        private string _newsFeedBaseUrl = "https://inshortsapi.vercel.app/news?category=";
        private string _newsFeedUrl;

        public async Task<string> GetNewsFeed(string category)
        {
            _newsFeedUrl = _newsFeedBaseUrl + category;
            var rawContent = await GetNewsContentAsync();
            if (rawContent == null)
                return string.Empty;

            NewsRootobject newsObject = JsonConvert.DeserializeObject<NewsRootobject>(rawContent);
            var htmlContent = new StringBuilder();
            string title, description, readmoreUrl;

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

            return htmlContent.ToString();
        }

        private async Task<string> GetNewsContentAsync()
        {
            try
            {
                var client = new HttpClient();
                var content = await client.GetStringAsync(_newsFeedUrl);
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
