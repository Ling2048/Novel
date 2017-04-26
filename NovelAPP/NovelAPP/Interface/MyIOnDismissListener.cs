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

namespace NovelAPP.Interface
{
    public class MyIOnDismissListener : Java.Lang.Object, PopupWindow.IOnDismissListener
    {
        public delegate void ExtendMethod();
        public ExtendMethod extentdMethod;

        public void OnDismiss()
        {
            extentdMethod();
            //throw new NotImplementedException();
        }
    }
}