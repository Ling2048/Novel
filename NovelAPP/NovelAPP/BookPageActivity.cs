using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.Widget;
using Android.Graphics;
using NovelWebSite;
using Android.Support.V7.App;
using Android.Database;

namespace NovelAPP
{
    [Activity(Label = "BookPageActivity")]
    public class BookPageActivity : AppCompatActivity
    {
        IList<Model.ChapterLink> chapterList;
        ListView chapterListView;
        private SwipeRefreshLayout refreshLayout;
        Button footBtn;
        ArrayAdapter adapter;
        string BookLink = "";
        IMenuItem keepItem;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.BookPage);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            Intent intent = this.Intent;
            String href = intent.GetBundleExtra("href").GetString("href");
            SupportActionBar.Title = intent.GetBundleExtra("href").GetString("title");
            BookLink = href;
            var progressbar = this.FindViewById<ProgressBar>(Resource.Id.progressBar2);

            chapterListView = this.FindViewById<ListView>(Resource.Id.ChapterList);
            chapterListView.ItemClick += new EventHandler<AdapterView.ItemClickEventArgs>(ListView_ItemClick);
            refreshLayout = this.FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefreshLayout);
            refreshLayout.SetColorSchemeColors(Color.Red, Color.Green, Color.Blue, Color.Yellow);
            refreshLayout.Refresh += (sender, e) =>
            {
                BookHelper.NovelInstance.GetBookPage(href, (model, ex) =>
                {
                    if (ex != null)
                    {
                        Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                        progressbar.Visibility = ViewStates.Gone;
                        return;
                    }
                    adapter.Clear();
                    chapterList = model.ChapterList;
                    foreach(string m in getData(model.ChapterList))
                    {
                        adapter.Add(m);
                    }
                    adapter.NotifyDataSetChanged();
                    refreshLayout.Refreshing = false;
                },0);
            };

            footBtn = new Button(this);
            footBtn.Text = "查看完整目录";
            footBtn.Click += (sender, e) => {
                Toast.MakeText(this, "正在加载完整目录！", ToastLength.Long).Show();

                BookHelper.NovelInstance.GetAllChapter(BookLink, (model, ex) => 
                {
                    if (ex != null)
                    {
                        Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                        progressbar.Visibility = ViewStates.Gone;
                        return;
                    }
                    chapterList = model.ChapterList;
                    adapter.Clear();
                    foreach (string m in getData(model.ChapterList))
                    {
                        adapter.Add(m);
                    }
                    chapterListView.Adapter = adapter;
                    adapter.NotifyDataSetChanged();
                });
            };

            progressbar.Visibility = ViewStates.Visible;

            BookHelper.NovelInstance.GetBookPage(href, (model,ex) => 
            {
                if (ex != null)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                    progressbar.Visibility = ViewStates.Gone;
                    return;
                }
                chapterList = model.ChapterList;
                adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleExpandableListItem1, getData(model.ChapterList));
                chapterListView.Adapter = adapter;
                chapterListView.AddFooterView(footBtn);
                progressbar.Visibility = ViewStates.Gone;
            },0);
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent();
            intent.SetClass(this, typeof(ChapterPage));
            Bundle b = new Bundle();
            b.PutString("href", chapterList[e.Position].URL.ToString());
            b.PutString("name", chapterList[e.Position].Name.ToString());
            intent.PutExtra("href", b);
            StartActivity(intent);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    this.Finish();
                    break;
                case Resource.Id.menu_keep:
                    string sql = "SELECT _id FROM KEEPBOOK WHERE website='{0}' AND bookurl='{1}' AND bookname='{2}'";
                    sql = string.Format(sql, BookHelper.NovelInstance.CurrentTypeName, BookLink, SupportActionBar.Title);
                    string id;
                    if (!string.IsNullOrEmpty(id = LocationSqliteOpenHelper.GetInstance(this).First_id(sql)))
                    {
                        Toast.MakeText(this, "取消收藏", ToastLength.Short).Show();
                        LocationSqliteOpenHelper.GetInstance(this).WritableDatabase.Delete("KEEPBOOK", "_id = ? ",new string[] { id });
                        keepItem.SetTitle("收藏");
                        return false;
                    }
                    Toast.MakeText(this, "收藏", ToastLength.Short).Show();
                    ContentValues cv = new ContentValues();
                    cv.Put("website", BookHelper.NovelInstance.CurrentTypeName);
                    cv.Put("bookurl", BookLink);
                    cv.Put("bookname", SupportActionBar.Title);
                    LocationSqliteOpenHelper.GetInstance(this).WritableDatabase.Insert("KEEPBOOK", null, cv);
                    keepItem.SetTitle("已收藏");
                    break;
                default:
                    Toast.MakeText(this, "Action selected: " + item.TitleFormatted,
                    ToastLength.Short).Show();
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        private List<String> getData(IList<Model.ChapterLink> list)
        {

            List<String> data = new List<string>();
            foreach (Model.ChapterLink link in list)
            {
                data.Add(link.Name);
            }
            return data;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Layout.bookpage_top_menus, menu);
            keepItem = menu.FindItem(Resource.Id.menu_keep);
            string sql = "SELECT _id FROM KEEPBOOK WHERE website='{0}' AND bookurl='{1}' AND bookname='{2}'";
            sql = string.Format(sql, BookHelper.NovelInstance.CurrentTypeName, BookLink, SupportActionBar.Title);
            if (LocationSqliteOpenHelper.GetInstance(this).Exists(sql)) keepItem.SetTitle("已收藏");
            return base.OnCreateOptionsMenu(menu);
        }


    }
}