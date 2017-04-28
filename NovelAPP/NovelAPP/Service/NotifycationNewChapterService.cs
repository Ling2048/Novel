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
using Android.Media;

namespace NovelAPP.Service
{
    [Service]
    public class NotifycationNewChapterService : Android.App.Service
    {
        private NotificationManager notificationManager;

        public override IBinder OnBind(Intent intent)
        {
            return null;
            //throw new NotImplementedException();
        }

        public override void OnCreate()
        {
            base.OnCreate();
            Handler handler = new Handler(MainLooper);
            handler.Post(() =>
            {
                Toast.MakeText(this, "启动666", ToastLength.Short).Show();
            });

            notificationManager = (NotificationManager)GetSystemService(NotificationService);
            Toast.MakeText(this, "服务启动", ToastLength.Short).Show();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            Handler handler = new Handler(MainLooper);
            handler.Post(() => 
            {
                Toast.MakeText(this, "启动", ToastLength.Short).Show();
            });

            //Toast.MakeText(this, "服务销毁", ToastLength.Short).Show();
            //Notification.Builder builder = new Notification.Builder(this);//新建Notification.Builder对象
            //PendingIntent intent1 = PendingIntent.GetActivity(this, 0, new Intent(this, typeof(MainActivity)), 0);
            ////PendingIntent点击通知后所跳转的页面
            //builder.SetContentTitle("标题"); //ContentTitle("Bmob Test");
            //builder.SetContentText("内容");
            //builder.SetSmallIcon(Resource.Drawable.Icon);
            //builder.SetContentIntent(intent1);//执行intent
            //Notification notification = builder.Build();//将builder对象转换为普通的notification
            //notification.Flags |= NotificationFlags.AutoCancel;//点击通知后通知消失
            //                                                   //获取系统默认的通知声音  
            //Android.Net.Uri ringUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            //notification.Sound = ringUri;
            //notificationManager.Notify(0, notification);

            //return StartCommandResult.NotSticky;
            return base.OnStartCommand(intent, flags, startId);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            Handler handler = new Handler(MainLooper);
            handler.Post(() =>
            {
                Toast.MakeText(this, "销毁666", ToastLength.Short).Show();
            });

            //Toast.MakeText(this, "服务销毁", ToastLength.Short).Show();
            //Notification.Builder builder = new Notification.Builder(this);//新建Notification.Builder对象
            //PendingIntent intent = PendingIntent.GetActivity(this, 0, new Intent(this, typeof(MainActivity)), 0);
            ////PendingIntent点击通知后所跳转的页面
            //builder.SetContentTitle("标题"); //ContentTitle("Bmob Test");
            //builder.SetContentText("内容");
            //builder.SetSmallIcon(Resource.Drawable.Icon);
            //builder.SetContentIntent(intent);//执行intent
            //Notification notification = builder.Build();//将builder对象转换为普通的notification
            //notification.Flags |= NotificationFlags.AutoCancel;//点击通知后通知消失
            //                                                   //获取系统默认的通知声音  
            //Android.Net.Uri ringUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            //notification.Sound = ringUri;
            //notificationManager.Notify(0, notification);
        }
    }
}