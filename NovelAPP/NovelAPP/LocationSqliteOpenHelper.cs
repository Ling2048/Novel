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
using Android.Database.Sqlite;
using Android.Database;

namespace NovelAPP
{
    public class LocationSqliteOpenHelper : SQLiteOpenHelper
    {
        static LocationSqliteOpenHelper helper;

        public static LocationSqliteOpenHelper GetInstance(Context context)
        {
            if (helper == null) helper = new LocationSqliteOpenHelper(context);
            return helper;
        }

        public LocationSqliteOpenHelper(Context context) : base(context, "cache", null, 1)
        {

        }
        public override void OnCreate(SQLiteDatabase db)
        {
            //查询缓存表
            db.ExecSQL("CREATE TABLE SEARCHCACHE(_id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,content TEXT NOT NULL)");
            //收藏表
            db.ExecSQL("CREATE TABLE KEEPBOOK(_id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,website TEXT NOT NULL,bookurl TEXT NOT NULL,bookname TEXT NOT NULL,updatetime TEXT NOT NULL)");
            //章节样式表
            db.ExecSQL("CREATE TABLE CHAPTERSTYLE(_id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,bgcolor TEXT NOT NULL,fontcolor TEXT NOT NULL,fontsize TEXT NOT NULL)");
            //throw new NotImplementedException();
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            //throw new NotImplementedException();
        }
        
        public bool Exists(string sql)
        {
            ICursor cursor = helper.ReadableDatabase.RawQuery(sql,null);
            return cursor.Count > 0 ? true : false;
        }

        public string First_id(string sql)
        {
            ICursor cursor = helper.ReadableDatabase.RawQuery(sql, null);
            cursor.MoveToFirst();
            if (cursor.Count <= 0) return null;
            return cursor.GetString(cursor.GetColumnIndex("_id"));
        }

        #region MyRegion
        public List<T> GetKeepList<T>(string sql)
        {
            ICursor icc = helper.WritableDatabase.RawQuery(sql, null);
            if (icc == null || !icc.MoveToFirst()) return null;

            List<T> list = new List<T>();
            Type t = typeof(T);
            object o = t.Assembly.CreateInstance(t.FullName);
            System.Reflection.PropertyInfo[] propertyInfos = o.GetType().GetProperties();
            do
            {
                for (int i = 0; i < icc.ColumnCount; i++)
                {
                    object oo = t.Assembly.CreateInstance(t.FullName);
                    foreach (System.Reflection.PropertyInfo property in propertyInfos)
                    {
                        if (icc.GetColumnName(i).Equals(property.Name))
                        {
                            property.SetValue(oo, icc.GetString(i), null);
                        }
                    }
                    list.Add((T)oo);
                }
            }
            while (icc.MoveToNext());
            return list;
        }

        public List<T> GetResultList<T>(string sql)
        {
            ICursor icc = helper.WritableDatabase.RawQuery(sql, null);
            if (icc == null || !icc.MoveToFirst()) return null;

            List<T> list = new List<T>();
            Type t = typeof(T);
            object o = t.Assembly.CreateInstance(t.FullName);
            System.Reflection.PropertyInfo[] propertyInfos = o.GetType().GetProperties();
            do
            {
                object oo = t.Assembly.CreateInstance(t.FullName);
                for (int i = 0; i < icc.ColumnCount; i++)
                {
                    foreach (System.Reflection.PropertyInfo property in propertyInfos)
                    {
                        if (icc.GetColumnName(i).Equals(property.Name))
                        {
                            property.SetValue(oo, icc.GetString(i), null);
                            continue;
                        }
                    }
                }
                list.Add((T)oo);
            }
            while (icc.MoveToNext());
            return list;
        }

        public TResult[] GetKeepListS<T, TResult>(string name, string sql)
        {
            List<T> list = GetKeepList<T>(sql);
            if (list == null || list.Count <= 0) return new TResult[0];
            System.Reflection.PropertyInfo[] t = typeof(T).GetProperties();
            var resultList = from item in list
                             select item.GetType().GetProperty(name).GetValue(item);
            return resultList.Cast<TResult>().ToArray();
        }

        public List<T> GetKeepList<T>()
        {
            ICursor icc = helper.WritableDatabase.RawQuery("SELECT * FROM KEEPBOOK ORDER BY _id DESC", null);
            if (icc == null || !icc.MoveToFirst()) return null;

            List<T> list = new List<T>();
            Type t = typeof(T);
            object o = t.Assembly.CreateInstance(t.FullName);
            System.Reflection.PropertyInfo[] propertyInfos = o.GetType().GetProperties();
            do
            {
                object oo = t.Assembly.CreateInstance(t.FullName);
                for (int i = 0; i < icc.ColumnCount; i++)
                {
                    foreach (System.Reflection.PropertyInfo property in propertyInfos)
                    {
                        if (icc.GetColumnName(i).Equals(property.Name.ToLower()))
                        {
                            string value = icc.GetString(i);
                            property.SetValue(oo, value, null);
                            continue;
                        }
                    }
                }
                list.Add((T)oo);
            }
            while (icc.MoveToNext());
            return list;
        }


        public TResult[] GetKeepListS<T, TResult>(string name)
        {
            List<T> list = GetKeepList<T>();
            if (list == null || list.Count <= 0) return new TResult[0];
            System.Reflection.PropertyInfo[] t = typeof(T).GetProperties();
            var resultList = from item in list
                             select item.GetType().GetProperty(name).GetValue(item);
            return resultList.Cast<TResult>().ToArray();
        }
        #endregion

        public string Test<T>()
        {
            ICursor icc = helper.WritableDatabase.RawQuery("SELECT * FROM KEEPBOOK ORDER BY _id DESC", null);
            if (icc == null || !icc.MoveToFirst()) return null;
            string s = "";
            List<T> list = new List<T>();
            Type t = typeof(T);
            object o = t.Assembly.CreateInstance(t.FullName);
            if (o == null) return "没有初始化";
            System.Reflection.PropertyInfo[] propertyInfos = o.GetType().GetProperties();
            do
            {
                object oo = t.Assembly.CreateInstance(t.FullName);
                if (oo == null) return "没有初始化";
                for (int i = 0; i < icc.ColumnCount; i++)
                {
                    foreach (System.Reflection.PropertyInfo property in propertyInfos)
                    {
                        //s += "3:" + icc.GetColumnName(i) + "::" + property.Name.ToLower();
                        //continue;
                        if (icc.GetColumnName(i).Equals(property.Name.ToLower()))
                        {
                            string value = icc.GetString(i);
                            s += property.Name + "=" + value + "|" + property.ToString() + "|" + property.Name + "|读" + property.CanRead + "|写" + property.CanWrite + "\n";
                            //continue;
                            property.SetValue(oo, value);
                            continue;
                        }
                    }
                }
                list.Add((T)oo);
            }
            while (icc.MoveToNext());
            return s;
        }
    }
}