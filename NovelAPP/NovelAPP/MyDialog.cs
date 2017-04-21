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
using static Android.App.ActionBar;

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

        public class Builder
        {
            private Context context;
            private String title;
            private String message;
            private String positiveButtonText;
            private String negativeButtonText;
            private View contentView;
            //private DialogInterface.OnClickListener positiveButtonClickListener;
            //private DialogInterface.OnClickListener negativeButtonClickListener;

            public Builder(Context context)
            {
                this.context = context;
            }

            public Builder setMessage(String message)
            {
                this.message = message;
                return this;
            }

            public Builder setTitle(int title)
            {
                this.title = (String)context.GetText(title);
                return this;
            }

            public Builder setTitle(String title)
            {
                this.title = title;
                return this;
            }

            public Builder setContentView(View v)
            {
                this.contentView = v;
                return this;
            }

            public MyDialog create()
            {
                int DialogId = Resource.Style.Dialog;
                int dialog_normal_layoutId = 0;
                int titleId = Resource.Id.title;
                int positiveButtonId = 0;
                int negativeButtonId = 0;
                int messageId = 0;
                int contentId = 0;

                LayoutInflater inflater = (LayoutInflater)context
                    .GetSystemService(Context.LayoutInflaterService);
                // instantiate the dialog with the custom Theme  
                MyDialog dialog = new MyDialog(context, DialogId);
                View layout = inflater.Inflate(dialog_normal_layoutId, null);
                dialog.AddContentView(layout, new LayoutParams(
                        LayoutParams.FillParent, LayoutParams.WrapContent));
                // set the dialog title  
                ((TextView)layout.FindViewById(titleId)).Text = title;
                if (positiveButtonText != null)
                {
                    ((Button)layout.FindViewById(positiveButtonId)).Text = positiveButtonText;
                    //if (positiveButtonClickListener != null)
                    //{

                    //}
                }
                else
                {
                    // if no confirm button just set the visibility to GONE  
                    layout.FindViewById(positiveButtonId).Visibility = ViewStates.Gone;
                }

                if (negativeButtonText != null)
                {
                }
                else
                {
                    // if no confirm button just set the visibility to GONE  
                    layout.FindViewById(negativeButtonId).Visibility = ViewStates.Gone;
                }

                if (message != null)
                {
                    ((TextView)layout.FindViewById(messageId)).Text = message;
                }
                else if (contentView != null)
                {
                    // if no message set  
                    // add the contentView to the dialog body  
                    ((LinearLayout)layout.FindViewById(contentId)).RemoveAllViews();
                    ((LinearLayout)layout.FindViewById(contentId))
                            .AddView(contentView, new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent));
                }
                dialog.SetContentView(layout);
                return dialog;
            }
        }
    }
}