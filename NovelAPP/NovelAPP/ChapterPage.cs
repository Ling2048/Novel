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

namespace NovelAPP
{
    [Activity(Label = "ChapterPage")]
    public class ChapterPage : AppCompatActivity
    {
        TextView contentView;
        IMenuItem themeBtn;
        Toolbar toolbar;
        LinearLayout ll;
        bool IsBlack = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
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
            contentView.Clickable = false;
            contentView.LongClickable = false;
            //contentView.SetTextColor()

            ll = this.FindViewById<LinearLayout>(Resource.Id.chapter_ll);
            //Android.Support.V7.Util.

            BookHelper.NovelInstance.GetChapterPage(href, (m,ex) => 
            {
                if (ex != null)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                    progressbar.Visibility = ViewStates.Gone;
                    return;
                }
                contentView.Text = Android.Text.Html.FromHtml(m.Content).ToString();
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
                        themeBtn.SetTitle("ºÚ");
                        break;
                    }
                    Helper.SetBlack(toolbar, ll, contentView);
                    IsBlack = true;
                    themeBtn.SetTitle("°×");
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

    }
}