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

namespace NovelAPP
{
    [Activity(Label = "ChapterPage",WindowSoftInputMode = SoftInput.AdjustUnspecified | SoftInput.StateHidden,ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation| Android.Content.PM.ConfigChanges.KeyboardHidden)]
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
            // ����״̬��
            
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
            popupWindow = InitPopupWindow();

            contentView.Click += (s, e) => 
            {
                if ((touchY > baseHeight && touchY < (baseHeight * 3)) && (touchX > baseWidth && touchX < baseWidth * 3) && !popupWindow.IsShowing)
                {
                    var rootView = LayoutInflater.From(this).Inflate(Resource.Layout.ChapterPage, null);
                    popupWindow.ShowAtLocation(rootView, GravityFlags.Bottom, 0, 0);
                    ////Toast.MakeText(this, "����˳ɹ�:TouchY:" + touchY + "|TouchX:" + touchX + "|" + baseHeight + "|" + baseWidth,
                    ////                ToastLength.Short).Show();
                    ////new MyDialog.Builder(this).create().Show();
                    //View popupView = this.LayoutInflater.Inflate(Resource.Layout.dialog_normal_layout, null);

                    ////popupView.FindViewById<EditText>(Resource.Id.font_color).Touch += (s1, e1) =>
                    ////{

                    ////};

                    //PopupWindow mPopupWindow = new PopupWindow(popupView, WindowManagerLayoutParams.MatchParent, WindowManagerLayoutParams.WrapContent);
                    //mPopupWindow.Touchable = true;
                    //mPopupWindow.OutsideTouchable = true;
                    ////���ÿ��Ի�ȡ���㣬���򵯳��˵��е�EditText���޷���ȡ�����
                    //mPopupWindow.Focusable = true;
                    ////�����Ϊ�˷�ֹ�����˵���ȡ����֮�󣬵��activity���������û����Ӧ
                    //mPopupWindow.SetBackgroundDrawable(new Android.Graphics.Drawables.BitmapDrawable());
                    //mPopupWindow.SoftInputMode = SoftInput.AdjustResize;// (WindowManager.LayoutParams.SOFT_INPUT_ADJUST_PAN);
                    //mPopupWindow.InputMethodMode = InputMethod.Needed;// (PopupWindow.INPUT_METHOD_NEEDED);


                    ////����PopupWindow����
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

            ll = this.FindViewById<LinearLayout>(Resource.Id.chapter_ll);

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
                        themeBtn.SetTitle("��");
                        break;
                    }
                    Helper.SetBlack(toolbar, ll, contentView);
                    IsBlack = true;
                    themeBtn.SetTitle("��");
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

        public PopupWindow InitPopupWindow()
        {
            View popupView = this.LayoutInflater.Inflate(Resource.Layout.dialog_normal_layout, null);



            PopupWindow mPopupWindow = new PopupWindow(popupView, WindowManagerLayoutParams.MatchParent, WindowManagerLayoutParams.WrapContent);
            mPopupWindow.ContentView.FindViewById<EditText>(Resource.Id.font_color).Click += (s, e) =>
            {
                Toast.MakeText(this, "������ɫ", ToastLength.Short).Show();
            };

            mPopupWindow.ContentView.FindViewById<EditText>(Resource.Id.bg_color).Click += (s, e) =>
            {
                Toast.MakeText(this, "������ɫ", ToastLength.Short).Show();
            };

            mPopupWindow.ContentView.FindViewById<Button>(Resource.Id.positiveButton).Click += (s, e) =>
            {
                Toast.MakeText(this, "ȷ��", ToastLength.Short).Show();
            };

            mPopupWindow.ContentView.FindViewById<Button>(Resource.Id.negativeButton).Click += (s, e) =>
            {
                Toast.MakeText(this, "ȡ��", ToastLength.Short).Show();
                popupWindow.Dismiss();
            };

            mPopupWindow.Touchable = true;
            mPopupWindow.OutsideTouchable = true;
            //���ÿ��Ի�ȡ���㣬���򵯳��˵��е�EditText���޷���ȡ�����
            mPopupWindow.Focusable = true;
            //�����Ϊ�˷�ֹ�����˵���ȡ����֮�󣬵��activity���������û����Ӧ
            mPopupWindow.SetBackgroundDrawable(new Android.Graphics.Drawables.BitmapDrawable());
            mPopupWindow.SoftInputMode = SoftInput.AdjustResize;// (WindowManager.LayoutParams.SOFT_INPUT_ADJUST_PAN);
            mPopupWindow.InputMethodMode = InputMethod.Needed;// (PopupWindow.INPUT_METHOD_NEEDED);

            MyIOnDismissListener dismissListener = new MyIOnDismissListener();
            dismissListener.extentdMethod = () => 
            {
                Toast.MakeText(this, "PopupWindow��ʧ", ToastLength.Short).Show();
            };
            mPopupWindow.SetOnDismissListener(dismissListener);

            //����PopupWindow����
            mPopupWindow.AnimationStyle = Resource.Style.anim_menu_bottombar;
            //mPopupWindow.ShowAsDropDown((Android.Views.View)sender);

            return mPopupWindow;
        }
    }
}