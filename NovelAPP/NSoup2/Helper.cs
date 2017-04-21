using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NSoup2
{
    public class Helper
    {

        public static object Get()
        {
            //下载网页源代码
            WebClient webClient = new WebClient();
            string htmlString = Encoding.GetEncoding("utf-8").GetString(webClient.DownloadData("http://zhannei.baidu.com/cse/search?q=%E6%83%8A%E6%82%9A%E4%B9%90%E5%9B%AD&s=11529447397045254029&srt=def&nsid=0"));
            NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(htmlString);
            NSoup.Select.Elements e = doc.GetElementsByClass("result-game-item-title-link");
            //获取Class为result-game-item-title-link的第一个的title属性的值
            string s = doc.Select(".result-game-item-title-link")[0].Attr("title");
            return doc;
        }

        public static IList<string> GetClassName(string ClassName,string AttrName,string htmlString)
        {
            NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(htmlString);
            NSoup.Select.Elements e = doc.GetElementsByClass(ClassName);
            List<string> list = new List<string>();
            foreach (NSoup.Nodes.Element element in e)
            {
                list.Add(element.Attr(AttrName));
            }
            return list;
        }

        public static IList<string> GetClassNameContent(string ClassName, string htmlString)
        {
            NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(htmlString);
            NSoup.Select.Elements e = doc.GetElementsByClass(ClassName);
            List<string> list = new List<string>();
            foreach (NSoup.Nodes.Element element in e)
            {
                list.Add(element.Text());
            }
            return list;
        }

        public static IList<string> Select(string SelectStr, string htmlString)
        {
            List<string> list = new List<string>();

            NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(htmlString);
            NSoup.Select.Elements e = doc.Select(SelectStr);
            foreach (NSoup.Nodes.Element element in e)
            {
                list.Add(element.Text());
            }
            return list;
        }

        public static IList<string> SelectHtml(string SelectStr, string htmlString)
        {
            List<string> list = new List<string>();

            NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(htmlString);
            NSoup.Select.Elements e = doc.Select(SelectStr);
            foreach (NSoup.Nodes.Element element in e)
            {
                list.Add(element.Html());
            }
            return list;
        }

        public static IList<string> Select(string SelectStr, string AttrName, string htmlString)
        {
            List<string> list = new List<string>();

            NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(htmlString);
            NSoup.Select.Elements e = doc.Select(SelectStr);
            foreach (NSoup.Nodes.Element element in e)
            {
                list.Add(element.Attr(AttrName));
            }
            return list;
        }

        public static string RemoveElement(string SelectStr, string htmlString)
        {
            NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(htmlString);
            NSoup.Select.Elements e = doc.Select(SelectStr);
            doc.Select("body").First().Attr("background-color", "yellow");
            string result = doc.Select("body").First().OuterHtml();
            foreach (NSoup.Nodes.Element element in e)
            {
                result = result.Replace(element.ToString(), "");
            }
            return result.Replace("body","div");
        }
    }
}
