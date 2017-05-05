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
using Android.Content.PM;
using Java.Lang;

namespace NovelAPP
{
    [Activity(MainLauncher = true, NoHistory = true, Theme = "@style/ThemeSplash",
       ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            this.SetContentView(Resource.Layout.SplashScreen);
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();

            //new Thread(() => 
            //{
                
            //}).Start();

        }
    }
}