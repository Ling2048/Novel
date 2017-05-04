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
using Java.Lang;
using Android.Media;

namespace NovelAPP
{
    public class Helper
    {
        //缓存路径
        //this.CacheDir.Path;
        public static string sdPath = Android.OS.Environment.ExternalStorageDirectory.Path;

        public static async void WriteCacheFile(Context context,string content,string cachePath,string fileName)
        {
            //string read = ReadCacheFile(context, cachePath, fileName);
            //if (read.Contains(content))
            //{
            //    Toast.MakeText(context, read + "分界线" + content, ToastLength.Short).Show();
            //    return;
            //}
            Java.IO.FileOutputStream file = new Java.IO.FileOutputStream(cachePath + "/" + fileName, true);
            await file.WriteAsync(System.Text.Encoding.UTF8.GetBytes(content + "|"));
            //Toast.MakeText(context, "已写入缓存：" + content, ToastLength.Short).Show();
            file.Close();
        }

        public static string ReadCacheFile(Context context, string fileName, string cachePath)
        {
            Java.IO.FileInputStream file = new Java.IO.FileInputStream(cachePath + "/" + fileName);
            byte[] b = new byte[1024];
            file.Read(b);
            file.Close();
            Toast.MakeText(context, System.Text.Encoding.UTF8.GetString(b), ToastLength.Short).Show();
            return System.Text.Encoding.UTF8.GetString(b);
        }

        public static View GetIdentifier(View view,string name)
        {
            //反射获取SearchView的输入框
            int id = view.Context.Resources.GetIdentifier("android:id/search_src_text", null, null);
            View v = view.FindViewById(id);
            return v;
        }

        public static List<string> GetDrawListData()
        {
            List<string> list = new List<string>
            {
                "收藏列表",
                "书签",
                "换源",
                "设置",
                "测试Notification",
                "启动Service",
                "停止Service",
                "检测Service是否启动",
                "抛异常"
            };
            return list;
        }

        public static void IntentActivity(Context context, Type type, Bundle bundle)
        {
            Intent intent = new Intent();
            intent.SetClass(context, type);
            intent.PutExtra("href", bundle);
            context.StartActivity(intent);
        }

        public static void Include()
        {
            NovelWebSite.Biquguan.com.Biquguan b = new NovelWebSite.Biquguan.com.Biquguan();
            Model.KeepModel k = new Model.KeepModel()
            {
                BookName = "",
                BookUrl = "",
                WebSite = ""
            };
        }

        public static void SetBlack(Android.Support.V7.Widget.Toolbar toolbar, LinearLayout ll, TextView contentView)
        {
            //ll.SetBackgroundResource(Resource.Drawable.abc_dialog_material_background_dark);
            ll.SetBackgroundColor(Android.Graphics.Color.DarkSlateGray);
            contentView.SetTextColor(Android.Graphics.Color.DarkKhaki);
            toolbar.SetBackgroundColor(Android.Graphics.Color.DarkSlateGray);
            //toolbar.SetBackgroundResource(Resource.Drawable.abc_dialog_material_background_dark);
        }

        public static void SetWhite(Android.Support.V7.Widget.Toolbar toolbar, LinearLayout ll,TextView contentView)
        {
            ll.SetBackgroundColor(Android.Graphics.Color.Azure);
            contentView.SetTextColor(Android.Graphics.Color.Black);
            toolbar.SetBackgroundColor(Android.Graphics.Color.DodgerBlue);
        }

        //通过反射获取状态栏高度，默认25dp
        public static int GetStatusBarHeight(Context context)
        {
            int statusBarHeight = dip2px(context, 25);
            try
            {
                Java.Lang.Class clazz = Java.Lang.Class.ForName("com.android.internal.R$dimen");
                var ob = clazz.NewInstance();
                int height = Integer.ParseInt(clazz.GetField("status_bar_height")
                        .Get(ob).ToString());
                statusBarHeight = context.Resources.GetDimensionPixelSize(height);
            }
            catch (System.Exception)
            {
                //e.StackTrace;
            }
            return statusBarHeight;
        }

        //根据手机的分辨率从 dp 的单位 转成为 px(像素)
        public static int dip2px(Context context, float dpValue)
        {
            float scale = context.Resources.DisplayMetrics.Density;
            return (int)(dpValue * scale + 0.5f);
        }

        public static void SendNotification(NotificationManager notificationManager,Context context,string title,string content, Bundle bundle)
        {
            Notification.Builder builder = new Notification.Builder(context);//新建Notification.Builder对象
            Intent intent = new Intent(context, typeof(BookPageActivity));
            intent.PutExtra("href", bundle);
            PendingIntent intent1 = PendingIntent.GetActivity(context, 0, intent, 0);
            //PendingIntent点击通知后所跳转的页面
            builder.SetContentTitle(title); //ContentTitle("Bmob Test");
            builder.SetContentText(content);
            builder.SetSmallIcon(Resource.Drawable.Icon);
            builder.SetContentIntent(intent1);//执行intent
            Notification notification = builder.Build();//将builder对象转换为普通的notification
            notification.Flags |= NotificationFlags.AutoCancel;//点击通知后通知消失
                                                               //获取系统默认的通知声音  
            Android.Net.Uri ringUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            notification.Sound = ringUri;
            notificationManager.Notify(0, notification);
        }
    }
}