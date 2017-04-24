using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using Android.Graphics;
using System.Collections.Generic;
using static Android.Widget.SimpleAdapter;
using Java.Lang;
using static NovelAPP.MyAdapter<System.Collections.Generic.IDictionary<string, object>>;
using Android.Support.V4.Widget;
using NovelWebSite;
using Android.Database;
using Android.Support.V7.App;
using V7Widget = Android.Support.V7.Widget;

namespace NovelAPP
{
    [Activity(Label = "NovelAPP", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity, V7Widget.SearchView.IOnQueryTextListener
    {
        int count = 1;
        IList<IDictionary<string, object>> SearchResultList = new List<IDictionary<string, object>>();
        MyAdapter<IDictionary<string, object>> adapter;
        ListView listview;
        private SwipeRefreshLayout refreshLayout;
        Button footBtn;
        string typeName = BookHelper.GetCSTypeList()[0];
        V7Widget.SearchView searchView;
        Android.Support.V4.Widget.SimpleCursorAdapter cursorAdapter;
        string QueryStr = "";
        ListView mDrawList;
        private Android.Support.V7.App.ActionBarDrawerToggle mDrawerToggle;
        private DrawerLayout mDrawerLayout;
        Android.Support.V7.Widget.Toolbar toolbar;
        ProgressBar progressbar;

        public bool OnQueryTextChange(string newText)
        {
            return true;
        }

        public bool OnQueryTextSubmit(string query)
        {
            LocationSqliteOpenHelper sqlHelper = LocationSqliteOpenHelper.GetInstance(this);
            Android.Database.Sqlite.SQLiteDatabase database = sqlHelper.WritableDatabase;
            ICursor ic = sqlHelper.WritableDatabase.Query("SEARCHCACHE", new string[] { "_id", "content" }, " content = ? ", new string[] { query }, null, null, null);
            if (ic.Count <= 0)
            {
                ContentValues cv = new ContentValues();
                cv.Put("content", query);
                database.Insert("SEARCHCACHE", null, cv);

                ICursor icc = sqlHelper.ReadableDatabase.RawQuery("SELECT * FROM SEARCHCACHE ORDER BY _id DESC limit 0,5", null);
                ic.MoveToFirst();
                cursorAdapter = new Android.Support.V4.Widget.SimpleCursorAdapter(this, Resource.Layout.SearchView_Suggestions_ItemView, 
                                                                                 icc, new string[] { "content" }, new int[] { Resource.Id.suggestion_tview });
                searchView.SuggestionsAdapter = cursorAdapter;
            }

            try
            {
                if (progressbar.Visibility == ViewStates.Visible) return false;
                progressbar.Visibility = ViewStates.Visible;


                BookHelper.NovelInstance.GetSearchList(query, (list, ex) =>
                {
                    if (ex != null)
                    {
                        Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                        progressbar.Visibility = ViewStates.Gone;
                        return;
                    }
                    SearchResultList = this.GetDictionary(list);
                    adapter.Clear();
                    adapter.AddAll(SearchResultList);

                    for (int i = 0; i < adapter.Count; i++)
                    {
                        JavaDictionary<string, object> d = (JavaDictionary<string, object>)adapter.GetItem2(i);
                        NovelWebSite.BookHelper.GetImageBitmapFromUrl(d["img"].ToString(), imageBytes =>
                        {
                            Bitmap imageBitmap;
                            imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                            d["img"] = imageBitmap;
                            adapter.NotifyDataSetChanged();
                        });
                    }
                    if (listview.FooterViewsCount < 1)
                        listview.AddFooterView(footBtn);
                    QueryStr = query;
                    progressbar.Visibility = ViewStates.Gone;
                }, 0);
            }
            catch (System.Exception e)
            {
                Toast.MakeText(this, e.Message, ToastLength.Long).Show();
            }
            return true;
        }

        IList<IDictionary<string, object>> GetDictionary(List<Model.SearchModel> list)
        {
            IList<IDictionary<string, object>> l = new List<IDictionary<string, object>>();

            foreach (Model.SearchModel m in list)
            {
                JavaDictionary<string, object> d = new JavaDictionary<string, object>();
                d.Add("title", m.Title);
                d.Add("info", m.Profiles);
                d.Add("img", m.PicHref);
                d.Add("Date", m.Date);
                d.Add("NewChapter", m.NewChapter);
                d.Add("BookLink", m.BookLink);
                l.Add(d);
            }
            return l;
        }


        void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Bundle b = new Bundle();
            JavaDictionary<string, object> d = (JavaDictionary<string, object>)adapter.GetItem2(e.Position);
            b.PutString("href", d["BookLink"].ToString());
            b.PutString("title", d["title"].ToString());
            b.PutString("cover", d["img"].ToString());
            Helper.IntentActivity(this, typeof(BookPageActivity), b);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Helper.Include();


            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            Init();
            //if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            //{
            //    Window.AddFlags(WindowManagerFlags.TranslucentStatus);
            //    toolbar.SetPadding(0, Helper.GetStatusBarHeight(this), 0, 0);
            //}
            //else
            //{
            //    Window.AddFlags(WindowManagerFlags.TranslucentNavigation);
            //}



            mDrawerToggle.SyncState();
            mDrawerLayout.AddDrawerListener(mDrawerToggle);

            mDrawList.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleExpandableListItem1, Helper.GetDrawListData());
            mDrawList.ItemClick += MDrawList_ItemClick;

            listview.ItemClick += new EventHandler<AdapterView.ItemClickEventArgs>(ListView_ItemClick);
            refreshLayout.SetColorSchemeColors(Color.Red, Color.Green, Color.Blue, Color.Yellow);
            refreshLayout.Refresh += (sender, e) =>
            {
                if (string.IsNullOrEmpty(QueryStr))
                {
                    refreshLayout.Refreshing = false;
                    return;
                }

                BookHelper.NovelInstance.GetSearchList(QueryStr, (list, ex) =>
                {
                    if (ex != null)
                    {
                        Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                        return;
                    }
                    SearchResultList = this.GetDictionary(list);
                    adapter.Clear();
                    adapter.AddAll(SearchResultList);
                    for (int i = 0; i < adapter.Count; i++)
                    {
                        JavaDictionary<string, object> d = (JavaDictionary<string, object>)adapter.GetItem2(i);
                        NovelWebSite.BookHelper.GetImageBitmapFromUrl(d["img"].ToString(), imageBytes =>
                        {
                            Bitmap imageBitmap;
                            imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                            d["img"] = imageBitmap;
                            adapter.NotifyDataSetChanged();
                        });
                    }
                    refreshLayout.Refreshing = false;
                },0);
            };

            adapter = new MyAdapter<IDictionary<string, object>>(this, SearchResultList,Resource.Layout.ListItem);
            adapter.InitDelegate += new InitListItem((position, convertView, parent, list) => {
                ImageView img = (ImageView)convertView.FindViewById(Resource.Id.img);
                TextView title = (TextView)convertView.FindViewById(Resource.Id.title);
                TextView info = (TextView)convertView.FindViewById(Resource.Id.info);
                TextView Date = (TextView)convertView.FindViewById(Resource.Id.Date);
                TextView NewChapter = (TextView)convertView.FindViewById(Resource.Id.NewChapter);
                img.SetImageBitmap((list[position] as JavaDictionary<string, object>)["img"] as Bitmap);
                title.Text = (list[position] as JavaDictionary<string, object>)["title"].ToString();
                info.Text = Android.Text.Html.FromHtml((list[position] as JavaDictionary<string, object>)["info"].ToString(),Android.Text.FromHtmlOptions.OptionUseCssColors).ToString();
                Date.Text = (list[position] as JavaDictionary<string, object>)["Date"].ToString();
                NewChapter.Text = (list[position] as JavaDictionary<string, object>)["NewChapter"].ToString();
            });
            listview.Adapter = adapter;

            footBtn = new Button(this);
            footBtn.Text = "加载更多";
            footBtn.Click += (sender, e) => {
                if (listview.Adapter.Count <= 0) { return; }
                Toast.MakeText(this, "正在加载第" + count + "页", ToastLength.Short).Show();

                BookHelper.NovelInstance.GetSearchList(QueryStr, (list, ex) =>
                {
                    if (ex != null)
                    {
                        Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                        return;
                    }
                    SearchResultList = this.GetDictionary(list);
                    adapter.AddAll(SearchResultList);
                    for (int i = (count * 10); i < adapter.Count; i++)
                    {
                        JavaDictionary<string, object> d = (JavaDictionary<string, object>)adapter.GetItem2(i);
                        NovelWebSite.BookHelper.GetImageBitmapFromUrl(d["img"].ToString(), imageBytes =>
                        {
                            Bitmap imageBitmap;
                            imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                            d["img"] = imageBitmap;
                            adapter.NotifyDataSetChanged();
                        });
                    }
                    refreshLayout.Refreshing = false;
                    count++;
                },count);
            };
        }

        private void MDrawList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            switch (e.Position)
            {
                case 0:
                    try
                    {
                        string[] source = LocationSqliteOpenHelper.GetInstance(this).GetKeepListS<Model.KeepModel, string>("BookName");
                        List<Model.KeepModel> list = LocationSqliteOpenHelper.GetInstance(this).GetKeepList<Model.KeepModel>();
                        NovelAPP.InterfaceInit.MyDialogInterface dialogInterface = new NovelAPP.InterfaceInit.MyDialogInterface(this, list);
                        dialogInterface.Click = (dialog, which) =>
                        {
                            Bundle b = new Bundle();
                            BookHelper.NovelInstance = Factory.GetWebInterface(list[which].WebSite);
                            b.PutString("href", list[which].BookUrl);
                            b.PutString("title", list[which].BookName);
                            Helper.IntentActivity(this, typeof(BookPageActivity), b);
                        };
                        View popupView = this.LayoutInflater.Inflate(Resource.Layout.dialog_normal_layout, null);

                        PopupWindow mPopupWindow = new PopupWindow(popupView, WindowManagerLayoutParams.MatchParent, WindowManagerLayoutParams.WrapContent);
                        mPopupWindow.Touchable = true;
                        mPopupWindow.OutsideTouchable = true;
                        mPopupWindow.ShowAsDropDown((Android.Views.View)sender);
                        //mPopupWindow.ShowAtLocation(FindViewById(Resource.Layout.Main), GravityFlags.Bottom, 0, 0);
                        //new Android.App.AlertDialog.Builder(this).SetTitle("收藏列表").SetItems(source, dialogInterface).Show();
                    }
                    catch (System.Exception ex)
                    {
                        Helper.WriteCacheFile(this, "StackTrace" + ex.StackTrace + "\n",this.CacheDir.Path,"debug.log");
                        Toast.MakeText(this, ex.Message + "|" + ex.GetType().FullName, ToastLength.Long).Show();
                    }
                    break;
                default:break;
            }
        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Layout.top_menus, menu);
            IMenuItem menuItem = menu.FindItem(Resource.Id.search);
            searchView = (V7Widget.SearchView)menuItem.ActionView;
            searchView.Iconified = true;
            searchView.SetIconifiedByDefault(false);
            searchView.SetOnQueryTextListener(this);

            LocationSqliteOpenHelper sqlHelper = new LocationSqliteOpenHelper(this);
            ICursor ic = sqlHelper.ReadableDatabase.RawQuery("SELECT * FROM SEARCHCACHE ORDER BY _id DESC limit 0,5", null);
            ic.MoveToFirst();
            cursorAdapter = new Android.Support.V4.Widget.SimpleCursorAdapter(this, Resource.Layout.SearchView_Suggestions_ItemView, ic, new string[] { "content" }, new int[] { Resource.Id.suggestion_tview });
            //坑
            searchView.SuggestionsAdapter = cursorAdapter;
            //坑
            searchView.SuggestionClick += SearchView_SuggestionClick;

            return base.OnCreateOptionsMenu(menu);
        }

        private void SearchView_SuggestionClick(object sender, V7Widget.SearchView.SuggestionClickEventArgs e)
        {
            ICursor c = (ICursor)cursorAdapter.GetItem(e.Position);
            string s = c.GetString(c.GetColumnIndex("content"));
            Toast.MakeText(this, s, ToastLength.Short).Show();
            searchView.FindViewById<V7Widget.SearchView.SearchAutoComplete>(Resource.Id.search_src_text).Text = s;
            this.OnQueryTextSubmit(s);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    this.Finish();
                    break;
                case Resource.Id.menu_CS:
                    Toast.MakeText(this, "换源", ToastLength.Short).Show();
                    NovelAPP.InterfaceInit.MyDialogInterface dialogInterface = new InterfaceInit.MyDialogInterface();
                    dialogInterface.Click = (dialog, which) =>
                    {
                        Toast.MakeText(this, BookHelper.GetCSTypeList()[which], ToastLength.Short).Show();
                        if (typeName == BookHelper.GetCSTypeList()[which]) return;
                        typeName = BookHelper.GetCSTypeList()[which];
                        this.OnQueryTextSubmit(searchView.Query);
                    };
                    new Android.App.AlertDialog.Builder(this).SetTitle("选择源").SetItems(BookHelper.GetCSNameList().ToArray(), dialogInterface).Show();
                    //new MyDialog.Builder(this).create().Show();
                    //mPopupWindow.ShowAtLocation(FindViewById(Resource.Layout.Main), GravityFlags.Bottom, 0, 0);
                    //new Android.App.AlertDialog.Builder(this).SetView(this.FindViewById<LinearLayout>(Resource.Layout.))
                    break;
                case Resource.Id.search:
                    Toast.MakeText(this, "查询", ToastLength.Short).Show();
                    break;
                default:
                    Toast.MakeText(this, "Action selected: " + item.TitleFormatted, ToastLength.Short).Show();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        bool isExit = true;

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                if (isExit)
                {
                    isExit = false;
                    System.ComponentModel.BackgroundWorker bw = new System.ComponentModel.BackgroundWorker();
                    Toast.MakeText(this, "再按一次退出程序", ToastLength.Long).Show();
                    bw.DoWork += (sender, ex) =>
                    {
                        Thread.Sleep(2000);
                        isExit = true;
                    };
                }
                else
                {
                    Finish();
                    JavaSystem.Exit(0);
                }
            }
            return false;
        }

        private void Bw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Init()
        {
            //获取小说网站操作类
            BookHelper.NovelInstance = NovelWebSite.Factory.GetWebInterface(typeName, "NovelWebSite");
            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.SetPadding(0, Helper.GetStatusBarHeight(this), 0, 0);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Xamarin.Android";
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.dl_left);
            mDrawerToggle = new Android.Support.V7.App.ActionBarDrawerToggle(this, mDrawerLayout, toolbar, Resource.String.open, Resource.String.close);
            mDrawList = this.FindViewById<ListView>(Resource.Id.lv_left_menu);
            listview = this.FindViewById<ListView>(Resource.Id.BookList);
            refreshLayout = this.FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefreshLayout);
            progressbar = this.FindViewById<ProgressBar>(Resource.Id.progressBar1);
        }
    }
}

