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
using NovelWebSite;
using Android.Support.V7.App;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using static Android.App.ActionBar;
using Android.Graphics;
using Android.Support.V4;
using Android.Util;
using NovelAPP.Interface;
using Android.Views.InputMethods;
using Android.Database;

namespace NovelAPP
{
    [Activity(Label = "ChapterPage",WindowSoftInputMode = SoftInput.StateAlwaysHidden | SoftInput.StateHidden,ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation| Android.Content.PM.ConfigChanges.KeyboardHidden)]
    public class ChapterPage : AppCompatActivity
    {
        TextView contentView;
        IMenuItem themeBtn;
        Toolbar toolbar;
        LinearLayout ll;
        bool IsBlack = false;
        int baseHeight = 0;
        int baseWidth = 0;
        float touchX = 0;
        float touchY = 0;
        PopupWindow popupWindow;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            // 隐藏状态栏
            
            Window.SetFlags(WindowManagerFlags.Fullscreen,
                WindowManagerFlags.Fullscreen);

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ChapterPage);
            // Create your application here

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            Intent intent = this.Intent;
            String href = intent.GetBundleExtra("href").GetString("href");
            SupportActionBar.Title = intent.GetBundleExtra("href").GetString("name");
            var progressbar = this.FindViewById<ProgressBar>(Resource.Id.progressBar3);
            progressbar.Visibility = ViewStates.Visible;
            contentView = this.FindViewById<TextView>(Resource.Id.ChapterContent);

            contentView.ScrollTo(0, 0);
            DisplayMetrics dm = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetMetrics(dm);
            baseWidth = dm.WidthPixels / 4;
            baseHeight = dm.HeightPixels / 4;

            ll = this.FindViewById<LinearLayout>(Resource.Id.chapter_ll);
            
            popupWindow = InitPopupWindow(InitStyle());

            contentView.Click += (s, e) => 
            {
                if ((touchY > baseHeight && touchY < (baseHeight * 3)) && (touchX > baseWidth && touchX < baseWidth * 3) && !popupWindow.IsShowing)
                {
                    var rootView = LayoutInflater.From(this).Inflate(Resource.Layout.ChapterPage, null);
                    popupWindow.ShowAtLocation(rootView, GravityFlags.Bottom, 0, 0);
                    ////Toast.MakeText(this, "点击了成功:TouchY:" + touchY + "|TouchX:" + touchX + "|" + baseHeight + "|" + baseWidth,
                    ////                ToastLength.Short).Show();
                    ////new MyDialog.Builder(this).create().Show();
                    //View popupView = this.LayoutInflater.Inflate(Resource.Layout.dialog_normal_layout, null);

                    ////popupView.FindViewById<EditText>(Resource.Id.font_color).Touch += (s1, e1) =>
                    ////{

                    ////};

                    //PopupWindow mPopupWindow = new PopupWindow(popupView, WindowManagerLayoutParams.MatchParent, WindowManagerLayoutParams.WrapContent);
                    //mPopupWindow.Touchable = true;
                    //mPopupWindow.OutsideTouchable = true;
                    ////设置可以获取焦点，否则弹出菜单中的EditText是无法获取输入的
                    //mPopupWindow.Focusable = true;
                    ////这句是为了防止弹出菜单获取焦点之后，点击activity的其他组件没有响应
                    //mPopupWindow.SetBackgroundDrawable(new Android.Graphics.Drawables.BitmapDrawable());
                    //mPopupWindow.SoftInputMode = SoftInput.AdjustResize;// (WindowManager.LayoutParams.SOFT_INPUT_ADJUST_PAN);
                    //mPopupWindow.InputMethodMode = InputMethod.Needed;// (PopupWindow.INPUT_METHOD_NEEDED);


                    ////设置PopupWindow动画
                    //mPopupWindow.AnimationStyle = Resource.Style.anim_menu_bottombar;
                    ////mPopupWindow.ShowAsDropDown((Android.Views.View)sender);
                    //var rootView = LayoutInflater.From(this).Inflate(Resource.Layout.ChapterPage, null);
                    //mPopupWindow.ShowAtLocation(rootView, GravityFlags.Bottom, 0, 0);
                }
            };
            //IList<View> list = contentView.Touchables;
            //contentView.SetOnTouchListener(new Interface.MyOnTouchListener());
            //contentView.Clickable = false;
            //contentView.LongClickable = false;
            //contentView.SetTextColor()

            BookHelper.NovelInstance.GetChapterPage(href, (m,ex) => 
            {
                if (ex != null)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                    progressbar.Visibility = ViewStates.Gone;
                    return;
                }
                contentView.Text = Android.Text.Html.FromHtml(m.Content,Android.Text.FromHtmlOptions.OptionUseCssColors).ToString();
                //contentView.Text = Android.Text.Html.EscapeHtml(m.Content);
                progressbar.Visibility = ViewStates.Gone;
            });
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    this.Finish();
                    break;
                case Resource.Id.menu_Night:
                    if (IsBlack)
                    {
                        Helper.SetWhite(toolbar, ll, contentView);
                        IsBlack = false;
                        themeBtn.SetTitle("黑");
                        break;
                    }
                    Helper.SetBlack(toolbar, ll, contentView);
                    IsBlack = true;
                    themeBtn.SetTitle("白");
                    break;
                default:
                    Toast.MakeText(this, "Action selected: " + item.TitleFormatted,
                                    ToastLength.Short).Show();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Layout.Chapter_top_menus, menu);
            themeBtn = menu.FindItem(Resource.Id.menu_Night);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            touchX = ev.GetX();
            touchY = ev.GetY();
            return base.DispatchTouchEvent(ev);
        }

        public PopupWindow InitPopupWindow(List<Model.ChapterStyleModel> list)
        {
            View popupView = this.LayoutInflater.Inflate(Resource.Layout.dialog_normal_layout, null);



            PopupWindow mPopupWindow = new PopupWindow(popupView, WindowManagerLayoutParams.MatchParent, WindowManagerLayoutParams.WrapContent);
            if (list != null)
            {
                mPopupWindow.ContentView.FindViewById<EditText>(Resource.Id.font_size).Text = list[0].fontsize;
                mPopupWindow.ContentView.FindViewById<EditText>(Resource.Id.font_color).Text = list[0].fontcolor;
                mPopupWindow.ContentView.FindViewById<EditText>(Resource.Id.bg_color).Text = list[0].bgcolor;
            }
            //mPopupWindow.ContentView.FindViewById<EditText>(Resource.Id.font_size).Click += (s, e) =>
            //{
            //    Toast.MakeText(this, "字号", ToastLength.Short).Show();
            //    //contentView.TextSize = Convert.ToSingle(((EditText)s).Text);
            //};

            //mPopupWindow.ContentView.FindViewById<EditText>(Resource.Id.font_color).Click += (s, e) =>
            //{
            //    Toast.MakeText(this, "字体色", ToastLength.Short).Show();
            //    //contentView.SetTextColor(new Color(Convert.ToInt32(((EditText)s).Text)));
            //};

            //mPopupWindow.ContentView.FindViewById<EditText>(Resource.Id.bg_color).Click += (s, e) =>
            //{
            //    Toast.MakeText(this, "背景色", ToastLength.Short).Show();
            //    //ll.SetBackgroundColor(new Color(Convert.ToInt32(((EditText)s).Text)));
            //};

            mPopupWindow.ContentView.FindViewById<Button>(Resource.Id.positiveButton).Click += (s, e) =>
            {
                Toast.MakeText(this, "确定", ToastLength.Short).Show();
                string fontSize = mPopupWindow.ContentView.FindViewById<EditText>(Resource.Id.font_size).Text;
                string fontColor = mPopupWindow.ContentView.FindViewById<EditText>(Resource.Id.font_color).Text;
                string bgColor = mPopupWindow.ContentView.FindViewById<EditText>(Resource.Id.bg_color).Text;

                SetStyle(bgColor, fontColor, fontSize);

                string sql = "SELECT _id FROM CHAPTERSTYLE";
                string id = LocationSqliteOpenHelper.GetInstance(this).First_id(sql);
                ContentValues cv = new ContentValues();
                if (!string.IsNullOrEmpty(id))
                {
                    cv.Put("bgcolor", bgColor);
                    cv.Put("fontcolor", fontColor);
                    cv.Put("fontsize", fontSize);
                    LocationSqliteOpenHelper.GetInstance(this).WritableDatabase.Update("CHAPTERSTYLE", cv, " _id = ? ", new string[] { id });
                    return;
                }
                cv.Put("bgcolor", bgColor);
                cv.Put("fontcolor", fontColor);
                cv.Put("fontsize", fontSize);
                LocationSqliteOpenHelper.GetInstance(this).WritableDatabase.Insert("CHAPTERSTYLE", null, cv);
            };

            mPopupWindow.ContentView.FindViewById<Button>(Resource.Id.negativeButton).Click += (s, e) =>
            {
                Toast.MakeText(this, "取消", ToastLength.Short).Show();
                popupWindow.Dismiss();
            };

            mPopupWindow.Touchable = true;
            mPopupWindow.OutsideTouchable = true;
            //设置可以获取焦点，否则弹出菜单中的EditText是无法获取输入的
            mPopupWindow.Focusable = true;
            //这句是为了防止弹出菜单获取焦点之后，点击activity的其他组件没有响应
            mPopupWindow.SetBackgroundDrawable(new Android.Graphics.Drawables.BitmapDrawable());
            mPopupWindow.SoftInputMode = SoftInput.AdjustResize;// (WindowManager.LayoutParams.SOFT_INPUT_ADJUST_PAN);
            mPopupWindow.InputMethodMode = Android.Widget.InputMethod.Needed;// (PopupWindow.INPUT_METHOD_NEEDED);

            MyIOnDismissListener dismissListener = new MyIOnDismissListener();
            dismissListener.extentdMethod = () => 
            {
                Toast.MakeText(this, "PopupWindow消失", ToastLength.Short).Show();
                InputMethodManager imm = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
                if (imm != null)
                {
                    Toast.MakeText(this, "输入法消失", ToastLength.Short).Show();
                    //imm.HideSoftInputFromWindow(mPopupWindow.ContentView.FindViewById<EditText>(Resource.Id.font_size).WindowToken, 0);
                    //imm.HideSoftInputFromWindow(mPopupWindow.ContentView.FindViewById<EditText>(Resource.Id.font_color).WindowToken, 0);
                    //imm.HideSoftInputFromWindow(mPopupWindow.ContentView.FindViewById<EditText>(Resource.Id.bg_color).WindowToken, 0);
                    imm.HideSoftInputFromWindow(popupView.WindowToken, 0);
                    //imm.ToggleSoftInput(0, HideSoftInputFlags.NotAlways);
                }
            };
            mPopupWindow.SetOnDismissListener(dismissListener);

            //设置PopupWindow动画
            mPopupWindow.AnimationStyle = Resource.Style.anim_menu_bottombar;
            //mPopupWindow.ShowAsDropDown((Android.Views.View)sender);

            return mPopupWindow;
        }

        public List<Model.ChapterStyleModel> InitStyle()
        {
            List<Model.ChapterStyleModel> list = LocationSqliteOpenHelper.GetInstance(this).GetResultList<Model.ChapterStyleModel>("SELECT * FROM CHAPTERSTYLE ORDER BY _id DESC limit 0,1");//.ReadableDatabase.RawQuery(, null);
            if (list != null && list.Count > 0)
            {
                SetStyle(list[0].bgcolor, list[0].fontcolor, list[0].fontsize);
                return list;
            }
            return null;
        }

        public void SetStyle(string bgColor,string fontColor,string fontSize)
        {
            if (!string.IsNullOrEmpty(fontSize) || !string.IsNullOrWhiteSpace(fontSize))
            {
                contentView.TextSize = Convert.ToSingle(fontSize);
            }
            if (!string.IsNullOrEmpty(fontColor) || !string.IsNullOrWhiteSpace(fontColor))
            {
                if (fontColor.Length == 8)
                {
                    contentView.SetTextColor(new Color(Color.ParseColor("#" + fontColor)));
                }
                else { }
            }
            if (!string.IsNullOrEmpty(bgColor) || !string.IsNullOrWhiteSpace(bgColor))
            {
                if (bgColor.Length == 8)
                {
                    ll.SetBackgroundColor(new Color(Color.ParseColor("#" + bgColor)));
                    //toolbar.SetBackgroundColor(new Color(Color.ParseColor("#" + bgColor)));
                }
                else { }
            }
            //string sql = "SELECT _id FROM CHAPTERSTYLE";
            //string id = LocationSqliteOpenHelper.GetInstance(this).First_id(sql);
            //ContentValues cv = new ContentValues();
            //if (!string.IsNullOrEmpty(id))
            //{
            //    cv.Put("bgcolor", bgColor);
            //    cv.Put("fontcolor", fontColor);
            //    cv.Put("fontsize", fontSize);
            //    LocationSqliteOpenHelper.GetInstance(this).WritableDatabase.Update("CHAPTERSTYLE", cv, " _id = ? ", new string[] { id });
            //    return;
            //}
            //cv.Put("bgcolor", bgColor);
            //cv.Put("fontcolor", fontColor);
            //cv.Put("fontsize", fontSize);
            //LocationSqliteOpenHelper.GetInstance(this).WritableDatabase.Insert("CHAPTERSTYLE", null, cv);
        }
    }
}