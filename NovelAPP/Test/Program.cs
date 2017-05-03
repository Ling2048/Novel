using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSoup2;
using Model;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string intStr = "更新：2017-05-03 12:26:56";
            string s1 = intStr.Split('：')[1].ToString();
            int i = intStr.Length;
            //NSoup2.Helper.RemoveElement("p", content);

            //Helper.Get();
            //string url = "http://m.biquguan.com/bqg806/";
            //string url = "http://m.biquguan.com/bqg806/2349080.html";
            NovelWebSite.Biquguan.com.Biquguan novel = new NovelWebSite.Biquguan.com.Biquguan();

            novel.GetChapterPage("/bqg806/2422035.html", (model, e) => 
            {
                Console.WriteLine("123");
            });

            novel.GetSearchList("惊悚乐园", (list,e) => 
            {
                string ss = list[0].Title;
                Console.WriteLine(ss);
            });
            //ChapterModel model;
            //novel.GetChapterPage(url, m => {
            //    model = m;
            //    Console.WriteLine(model.Content);
            //});
            //BookPageModel BookList;
            //novel.GetBookPage(url, list => {
            //    BookList = list;
            //    Console.WriteLine(BookList.Review);
            //});
            //   try
            //   {
            //       //IList<SearchModel> searchList;
            //       //novel.GetSearchList("异常生物见闻录", list =>
            //       //{
            //       //    searchList = list;
            //       //    Console.WriteLine(searchList[0].Title);
            //       //});
            //       NovelWebSite.Biquguan.com.Biquguan.GetEx(() => 
            //       {
            //           Console.WriteLine("无异常");
            //       });
            //   }
            //   catch (Exception e)
            //{
            //       Console.WriteLine("异常");
            //   }

            //System.Threading.Tasks.TaskScheduler.UnobservedTaskException += (sender, e) =>
            //{
            //    string s = "";
            //};
            //List<Model.KeepModel> list = GetKeepList<Model.KeepModel>();
            string[] s = GetKeepListS<Model.KeepModel,string>("WebSite");
            Console.WriteLine("异步未完成");
            Console.ReadKey();
        }

        public static List<T> GetKeepList<T>()
        {
            List<T> list = new List<T>();
            Type t = typeof(T);
            object d = t.Assembly.CreateInstance(t.FullName);
            System.Reflection.PropertyInfo[] p = d.GetType().GetProperties();
            for (int i = 0,j = 1; i < 10; i++)
            {
                object o = t.Assembly.CreateInstance(t.FullName);
                foreach (System.Reflection.PropertyInfo pp in p)
                {
                    pp.SetValue(o, i + ":" + j++, null);
                }
                list.Add((T)o);
            }
            return list;
        }

        public static M[] GetKeepListS<T,M>(string name)
        {
            List<T> list = GetKeepList<T>();
            System.Reflection.PropertyInfo[] t = typeof(T).GetProperties();
            System.Reflection.PropertyInfo p = list[0].GetType().GetProperty(name);
            object o = list[0].GetType().GetProperty(name).GetValue(list[0]);
            var resultList = from item in list
                             //where item.GetType().GetProperty(name).Name == name
                             select item.GetType().GetProperty(name).GetValue(item);
            return resultList.Cast<M>().ToArray();
        }
    }
}
