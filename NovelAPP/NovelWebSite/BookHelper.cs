using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace NovelWebSite
{
    public class BookHelper
    {
        public static List<string> Group_List = new List<string> { "收藏" };

        private static IDictionary<string, string> CS_List = new Dictionary<string, string>() {
            { "笔趣馆","NovelWebSite.Biquguan.com.Biquguan" }
        };

        public static List<string> GetCSNameList()
        {
            return CS_List.Keys.ToList<string>();
        }

        public static List<string> GetCSTypeList()
        {
            return CS_List.Values.ToList<string>();
        }

        public static _NovelWebInterface NovelInstance = null;
        public static async void GetImageBitmapFromUrl(string url,Action<byte[]> CallBack)
        {
            byte[] imageBytes;
            using (var webClient = new WebClient())
            {
                imageBytes = await webClient.DownloadDataTaskAsync(new System.Uri(url));
            }

            CallBack(imageBytes);
        }
    }
}
