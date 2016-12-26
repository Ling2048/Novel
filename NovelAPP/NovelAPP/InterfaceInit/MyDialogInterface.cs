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

namespace NovelAPP.InterfaceInit
{
    public class MyDialogInterface : Java.Lang.Object,IDialogInterfaceOnClickListener
    {
        Context context;
        List<Model.KeepModel> list;
        public Action<IDialogInterface,int> Click;

        public MyDialogInterface()
        {
        }

        public MyDialogInterface(Context context, List<Model.KeepModel> list)
        {
            this.context = context;
            this.list = list;
        }

        public void OnClick(IDialogInterface dialog, int which)
        {
            Click.Invoke(dialog,which);
            //Bundle b = new Bundle();
            //BookHelper.NovelInstance = Factory.GetWebInterface(list[which].WebSite);
            //b.PutString("href", list[which].BookUrl);
            //b.PutString("title", list[which].BookName);
            //MainActivity.IntentActivity(context, typeof(BookPageActivity), b);
            //throw new NotImplementedException();
        }
    }
}