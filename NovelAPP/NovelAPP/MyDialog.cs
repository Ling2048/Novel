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

namespace NovelAPP
{ 
    public class MyDialog : Dialog
    {
        public MyDialog(Context context) : base(context)
        {
        }

        public MyDialog(Context context, int theme) : base(context,theme)
        {
        }

        //public class Builder
        //{
        //    private Context context;
        //    private String title;
        //    private String message;
        //    private String positiveButtonText;
        //    private String negativeButtonText;
        //    private View contentView;
        //    private DialogInterface.OnClickListener positiveButtonClickListener;
        //    private DialogInterface.OnClickListener negativeButtonClickListener;

        //    public Builder(Context context) {  
        //    this.context = context;  
        //}  
  
        //public Builder setMessage(String message) {  
        //    this.message = message;  
        //    return this;  
        //}  
        //}
    }
}