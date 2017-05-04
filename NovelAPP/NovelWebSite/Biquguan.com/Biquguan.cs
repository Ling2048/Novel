using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace NovelWebSite.Biquguan.com
{
    public class Biquguan : _NovelWebInterface
    {
        //string SearchWebSite = "http://zhannei.baidu.com/cse/search?q=%E6%83%8A%E6%82%9A%E4%B9%90%E5%9B%AD&s=11529447397045254029&srt=def&nsid=0";
        string SearchWebSite = "http://zhannei.baidu.com/cse/search?q={0}&s=11529447397045254029&srt=def&nsid=0";

        public override async void GetSearchList(string BookName,Action<List<SearchModel>,Exception> CallBack,int index = 0)
        {
            string url = string.Format(SearchWebSite, BookName);
            url = Uri.EscapeUriString(url);
            List<SearchModel> SearchList = new List<SearchModel>();
            string htmlString = "";
            try
            {
                htmlString = await this.GetWebSource(url + "&p=" + index);
            }
            catch (Exception e)
            {
                CallBack(null, e);
                return;
            }
            IList<string> TitleList = new List<string>();
            IList<string> ContentList = new List<string>();
            IList<string> DateList = new List<string>();
            IList<string> NewChapterList = new List<string>();
            IList<string> NewChapterHrefList = new List<string>();
            IList<string> BookLinkList = new List<string>();
            IList<string> PicHrefList = new List<string>();
            
            IList<string> item = NSoup2.Helper.SelectHtml("#results > div.result-list > div", htmlString);
            try
            {
                foreach (string i in item)
                {
                    SearchModel model = new SearchModel()
                    {
                        Title = NSoup2.Helper.GetClassName("result-game-item-title-link", "title", i).Count > 0 ? NSoup2.Helper.GetClassName("result-game-item-title-link", "title", i)[0] : "",
                        Profiles = NSoup2.Helper.SelectHtml("div.result-game-item-detail > p", i).Count > 0 ? NSoup2.Helper.SelectHtml("div.result-game-item-detail > p", i)[0] : "",
                        Date = NSoup2.Helper.Select("div.result-game-item-detail > div > p + p + p > span + span", i).Count > 0 ? NSoup2.Helper.Select("div.result-game-item-detail > div > p + p + p > span + span", i)[0] : "",
                        NewChapter = NSoup2.Helper.Select("div.result-game-item-detail > div > p + p + p + p > a", i).Count > 0 ? NSoup2.Helper.Select("div.result-game-item-detail > div > p + p + p + p > a", i)[0] : "",
                        NewChapterHref = NSoup2.Helper.Select("div.result-game-item-detail > div > p + p + p + p > a", "href", i).Count > 0 ? NSoup2.Helper.Select("div.result-game-item-detail > div > p + p + p + p > a", "href", i)[0] : "",
                        BookLink = NSoup2.Helper.Select("div.result-game-item-pic > a", "href", i).Count > 0 ? NSoup2.Helper.Select("div.result-game-item-pic > a", "href", i)[0] : "",
                        PicHref = NSoup2.Helper.Select("div.result-game-item-pic > a > img", "src", i).Count > 0 ? NSoup2.Helper.Select("div.result-game-item-pic > a > img", "src", i)[0] : ""
                    };
                    SearchList.Add(model);
                }
            }
            catch (Exception e)
            {
                CallBack(null, e);
                return;
            }
            CallBack(SearchList,null);
        }

        public override async void GetBookPage(string URL, Action<BookPageModel, Exception> CallBack, int index = 0)
        {
            URL = URL.Replace("www", "m");
            string htmlString = "";
            try
            {

                htmlString = await this.GetWebSource(URL);
            }
            catch (Exception e)
            {
                CallBack(null, e);
                return;
            }
            IList<string> synopsisArea_detailList = NSoup2.Helper.Select("body > div.synopsisArea > div.synopsisArea_detail > p", htmlString);
            IList<string> authorList = NSoup2.Helper.Select("body > div.synopsisArea > div.synopsisArea_detail > a > p", htmlString);
            IList<string> picHrefList = NSoup2.Helper.Select("body > div.synopsisArea > div.synopsisArea_detail > img", "src", htmlString);
            IList<string> reviewList = NSoup2.Helper.Select("body > div.synopsisArea > p.review", htmlString);
            IList<string> titleList = NSoup2.Helper.Select("body > header > span", htmlString);
            IList<string> chapterNameList = NSoup2.Helper.Select("#chapterlist > p", htmlString);
            IList<string> chapterHrefList = NSoup2.Helper.Select("#chapterlist > p > a", "href", htmlString);
            List<ChapterLink> chapterList = new List<ChapterLink>();
            try
            {
                if (chapterNameList.Count <= 0 || chapterHrefList.Count <= 0)
                    throw new Exception("并没有抓到数据！");
            }
            catch (Exception e)
            {
                CallBack(null, e);
                return;
            }
            for (int i = chapterNameList.Count - 1; i >= 0; i--)
            {
                ChapterLink link = new ChapterLink() { Name = chapterNameList[i], URL = chapterHrefList[i] };
                chapterList.Add(link);
            }
            BookPageModel model = new BookPageModel() { Author = authorList[0].Split('：')[1].ToString().Trim(), NewDateTime = synopsisArea_detailList[2].Split('：')[1].ToString().Trim(), NewChapterName = synopsisArea_detailList[3].Split('：')[1].ToString().Trim(), PicHref = picHrefList[0], Review = reviewList[0], ChapterList = chapterList, Title = titleList[0] };
            CallBack(model,null);
            //throw new NotImplementedException();
        }

        public override async void GetChapterPage(string URL, Action<ChapterModel, Exception> CallBack)
        {
            //URL = URL.Replace("www", "m");
            URL = "http://m.biquguan.com" + URL;
            string htmlString = "";
            try
            {

                htmlString = await this.GetWebSource(URL);
            }
            catch (Exception e)
            {
                CallBack(null, e);
                return;
            }
            IList<string> chapterContentList = NSoup2.Helper.SelectHtml("#chaptercontent", htmlString);
            try
            {
                if (chapterContentList.Count <= 0) throw new Exception("并没有抓到数据！");
            }
            catch (Exception e)
            {
                CallBack(null, e);
                return;
            }
            ChapterModel model = new ChapterModel() { Content = NSoup2.Helper.RemoveElement("p", chapterContentList[0]) };
            CallBack(model,null);
            //throw new NotImplementedException();
        }

        public override async void GetAllChapter(string URL, Action<BookPageModel, Exception> CallBack)
        {
            URL = URL.Replace("www", "m") + "/all.html";
            string htmlString = "";
            try
            {

                htmlString = await this.GetWebSource(URL);
            }
            catch (Exception e)
            {
                CallBack(null, e);
                return;
            }
            IList<string> chapterNameList = NSoup2.Helper.Select("#chapterlist > p", htmlString);
            IList<string> chapterHrefList = NSoup2.Helper.Select("#chapterlist > p > a", "href", htmlString);
            List<ChapterLink> chapterList = new List<ChapterLink>();
            try
            {
                if (chapterNameList.Count <= 0 || chapterHrefList.Count <= 0)
                    throw new Exception("并没有抓到数据！");
            }
            catch (Exception e)
            {
                CallBack(null, e);
                return;
            }
            for (int i = chapterNameList.Count - 1; i >= 0; i--)
            {
                ChapterLink link = new ChapterLink() { Name = chapterNameList[i], URL = chapterHrefList[i] };
                chapterList.Add(link);
            }
            BookPageModel model = new BookPageModel() { Review = "", ChapterList = chapterList, Title = "" };
            CallBack(model, null);
            //throw new NotImplementedException();
        }
    }
}
