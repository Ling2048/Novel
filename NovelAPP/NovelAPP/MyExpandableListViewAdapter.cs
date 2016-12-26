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

namespace NovelAPP
{
    public class MyExpandableListViewAdapter : BaseExpandableListAdapter
    {
        private JavaDictionary<string,List<string>> dataset = new JavaDictionary<string, List<string>>();

        private List<string> GroupList;
        private List<List<string>> ChildList;
        private Context context;

        public MyExpandableListViewAdapter(Context context,List<string> Group,List<List<string>> Child)
        {
            this.context = context;
            this.GroupList = Group;
            this.ChildList = Child;
        }

        public override int GroupCount
        {
            get
            {
                return dataset.Count;
                //throw new NotImplementedException();
            }
        }

        public override bool HasStableIds
        {
            get
            {
                return false;
                //throw new NotImplementedException();
            }
        }

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            return null;
            //return dataset[groupPosition];
            //throw new NotImplementedException();
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
            throw new NotImplementedException();
        }

        public override int GetChildrenCount(int groupPosition)
        {
            return dataset.Count;
            //throw new NotImplementedException();
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                convertView = LayoutInflater.From(context).Inflate(Resource.Layout.child_item, null);
            }
            TextView text = convertView.FindViewById<TextView>(Resource.Id.child_title);
            text.Text = ChildList[groupPosition][childPosition];
            return convertView;
            //throw new NotImplementedException();
        }


        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            return null;
            //throw new NotImplementedException();
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
            //throw new NotImplementedException();
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                convertView = LayoutInflater.From(context).Inflate(Resource.Layout.parent_item, null);
            }
            TextView text = convertView.FindViewById<TextView>(Resource.Id.parent_title);
            text.Text = GroupList[groupPosition];
            return convertView;
            //throw new NotImplementedException();
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return false;
            //throw new NotImplementedException();
        }
    }
}