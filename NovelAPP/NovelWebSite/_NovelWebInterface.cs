using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NovelWebSite
{
    public abstract class _NovelWebInterface 
    {
        protected async Task<string> GetWebSource(string URL)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.Credentials = CredentialCache.DefaultCredentials;
                webClient.Proxy = null;
                webClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                string htmlString = Encoding.GetEncoding("utf-8").GetString(await webClient.DownloadDataTaskAsync(new System.Uri(URL)));
                return htmlString;
            }
            catch (Exception e)
	        {
                throw new Exception("网络有问题！" + e.Message);
            }
        }

        public string CurrentTypeName { get; set; }
        public abstract void GetSearchList(string BookName,Action<List<Model.SearchModel>, Exception> CallBack, int index);
        public abstract void GetBookPage(string URL, Action<Model.BookPageModel, Exception> CallBack, int index);
        public abstract void GetChapterPage(string URL, Action<Model.ChapterModel, Exception> CallBack);
        public abstract void GetAllChapter(string URL, Action<Model.BookPageModel, Exception> CallBack);
    }
}
