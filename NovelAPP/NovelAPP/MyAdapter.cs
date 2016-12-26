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
    public class MyAdapter<T> : BaseAdapter
    {
        public delegate void InitListItem(int position, View listItem, ViewGroup parent, IList<T> list);
        public InitListItem InitDelegate;
        private IList<T> list = new List<T>();
        private Context context;
        private long resId;

        public MyAdapter(Context context,IList<T> list,long resId)
        {
            this.context = context;
            this.list = list;
            this.resId = resId;
        }


        public void Add(T item)
        {
            list.Add(item);
            NotifyDataSetChanged();
        }

        public void AddAll(IList<T> list)
        {
            foreach (T item in list)
            {
                this.Add(item);
            }
        }

        public void Clear()
        {
            list.Clear();
        }

        public void Remove(int position)
        {
            list.Remove(list[position]);
            NotifyDataSetChanged();
        }

        public override int Count
        {
            get
            {
                return list.Count;
                //throw new NotImplementedException();
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            //return (object)list[position];
            return null;
            //throw new NotImplementedException();
        }

        public T GetItem2(int position)
        {
            return list[position];
        }

        public override long GetItemId(int position)
        {
            return position;
            //throw new NotImplementedException();
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                convertView = LayoutInflater.From(context).Inflate(Convert.ToInt32(resId), null);
            }
            InitDelegate.Invoke(position, convertView, parent, list);
            return convertView;
            //return null;
        }


        //public View GetView(int position, View convertView, ViewGroup parent,Action<View> CallBack)
        //{
        //    //throw new NotImplementedException();
        //    if (convertView == null)
        //    {

        //        convertView = LayoutInflater.From(context).Inflate(Resource.Layout.ListItem, null);

        //    }
        //    //CallBack(convertView);
        //    InitDelegate.Invoke(position, parent, parent);
        //    return convertView;
        //}
    }
}