using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NovelWebSite
{
    public class Factory
    {
        private static IDictionary<string, object> di = new Dictionary<string, object>();

        //public static _NovelWebInterface GetWebInterface(string typeName, string assemblyString = "NovelWebSite")
        //{
        //    if (di.ContainsKey(typeName))
        //        return (_NovelWebInterface)di[typeName];
        //    switch (typeName)
        //    {
        //        case "NovelWebSite.Biquguan.com.Biquguan":
        //            _NovelWebInterface instance = new Biquguan.com.Biquguan();
        //            di.Add(typeName, instance);
        //            return instance;
        //        default:
        //            return null;
        //    }
        //}

        public static _NovelWebInterface GetWebInterface(string typeName, string assemblyString = "NovelWebSite")
        {
            try
            {
                if (di.ContainsKey(typeName))
                    return (_NovelWebInterface)di[typeName];
                _NovelWebInterface instance = (_NovelWebInterface)Assembly.Load(assemblyString).CreateInstance(typeName);
                instance.CurrentTypeName = typeName;
                di.Add(typeName, instance);
                return instance;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
