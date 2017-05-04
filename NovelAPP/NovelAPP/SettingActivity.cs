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
using Android.Content.PM;

namespace NovelAPP
{
    [Activity(Label = "SettingActivity")]
    public class SettingActivity : AppCompatActivity,CompoundButton.IOnCheckedChangeListener
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
            Model.SettingInfoModel settingInfoModel = LocationSqliteOpenHelper.GetInstance(this).GetResultList<Model.SettingInfoModel>("SELECT isnotify FROM SETTINGINFO")[0];

            var listview = FindViewById<ListView>(Resource.Id.setting_list);
            MyAdapter<IDictionary<string, object>> adapter = new MyAdapter<IDictionary<string, object>>(this, GetDictionary(), Resource.Layout.setting_listitem);
            adapter.InitDelegate += new MyAdapter<IDictionary<string, object>>.InitListItem((position, convertView, parent, list) => {
                TextView title = (TextView)convertView.FindViewById(Resource.Id.title);
                title.Text = (list[position] as JavaDictionary<string, object>)["title"].ToString();
                Switch btnSwitch = convertView.FindViewById<Switch>(Resource.Id.isSendUpdate);
                if (title.Text.Equals("推送更新"))
                {
                    btnSwitch.Checked = settingInfoModel.isnotify.Equals("1");
                }
                btnSwitch.Tag = title.Text;
                btnSwitch.SetOnCheckedChangeListener(this);
            });
            listview.Adapter = adapter;

            //listview.Adapter.

            //IDictionary<string, object>[] dics = GetDictionary().ToArray();
            //// Define the query expression.
            //IDictionary<string, object> resultDic =
            //    (from dic in dics
            //    where dic["title"].ToString().Equals("推送更新")
            //    select dic).First();
            //int index = GetDictionary().IndexOf(resultDic);
            ////listview.Adapter.GetItem()
        }

        IList<IDictionary<string, object>> GetDictionary()
        {
            IList<IDictionary<string, object>> l = new List<IDictionary<string, object>>();

            JavaDictionary<string, object> d = new JavaDictionary<string, object>
            {
                { "title", "推送更新" }
            };
            l.Add(d);

            return l;
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

        public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
        {
            string id = LocationSqliteOpenHelper.GetInstance(this).First_id("SELECT _id FROM SETTINGINFO");
            switch (buttonView.Tag.ToString())
            {
                case "推送更新":
                    if (isChecked)
                    {
                        //推送
                        Intent mIntent = new Intent(this, typeof(NovelAPP.Service.NotifycationNewChapterService));
                        mIntent.PutExtra("Time", 1000 * 1);
                        ComponentName cn = StartService(new Intent(GetExplicitIntent(this, mIntent)));
                        Toast.MakeText(this, this.PackageName, ToastLength.Long).Show();
                        //更新数据库
                        ContentValues cv = new ContentValues();
                        cv.Put("isnotify", "1");
                        LocationSqliteOpenHelper.GetInstance(this).WritableDatabase.Update("SETTINGINFO", cv, "_id = ?", new string[] { id });
                        Toast.MakeText(this, "推送", ToastLength.Short).Show();
                    }
                    else
                    {
                        //销毁推送
                        StopService(new Intent(this, typeof(NovelAPP.Service.NotifycationNewChapterService)));
                        //更新数据库
                        ContentValues cv = new ContentValues();
                        cv.Put("isnotify", "0");
                        LocationSqliteOpenHelper.GetInstance(this).WritableDatabase.Update("SETTINGINFO", cv, "_id = ?", new string[] { id });
                        Toast.MakeText(this, "不推送", ToastLength.Short).Show();
                    }
                    break;
            }
            //throw new NotImplementedException();
        }

        public static Intent GetExplicitIntent(Context context, Intent implicitIntent)
        {
            // Retrieve all services that can match the given intent
            PackageManager pm = context.PackageManager;
            IList<ResolveInfo> resolveInfo = pm.QueryIntentServices(implicitIntent, 0);
            // Make sure only one match was found
            if (resolveInfo == null || resolveInfo.Count != 1)
            {
                return null;
            }
            // Get component info and create ComponentName
            ResolveInfo serviceInfo = resolveInfo[0];
            string packageName = serviceInfo.ServiceInfo.PackageName;
            string className = serviceInfo.ServiceInfo.Name;
            ComponentName component = new ComponentName(packageName, className);
            // Create a new intent. Use the old one for extras and such reuse
            Intent explicitIntent = new Intent(implicitIntent);
            // Set the component to be explicit
            explicitIntent.SetComponent(component);
            return explicitIntent;
        }
    }
}