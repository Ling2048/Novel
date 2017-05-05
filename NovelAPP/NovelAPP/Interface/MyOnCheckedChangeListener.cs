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
    public class MyOnCheckedChangeListener : Java.Lang.Object, CompoundButton.IOnCheckedChangeListener
    {
        Action<CompoundButton, bool> OnCheckedChangedAction;

        private MyOnCheckedChangeListener() { }

        public MyOnCheckedChangeListener(Action<CompoundButton, bool> onCheckedChangedAction)
        {
            OnCheckedChangedAction = onCheckedChangedAction;
        }

        public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
        {
            OnCheckedChangedAction.Invoke(buttonView, isChecked);
            //throw new NotImplementedException();
        }
    }
}