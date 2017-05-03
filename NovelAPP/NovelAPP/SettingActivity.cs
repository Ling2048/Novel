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
using Android.Support.V7.App;

namespace NovelAPP
{
    [Activity(Label = "SettingActivity")]
    public class SettingActivity : AppCompatActivity
    {
        Android.Support.V7.Widget.Toolbar toolbar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Setting);

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.SetPadding(0, Helper.GetStatusBarHeight(this), 0, 0);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "设置";
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            // Create your application here
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    this.Finish();
                    break;
                default:
                    Toast.MakeText(this, "Action selected: " + item.TitleFormatted,
                    ToastLength.Short).Show();
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}