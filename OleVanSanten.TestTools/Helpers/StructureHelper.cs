using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Linq;

namespace OleVanSanten.TestTools.Helpers
{
    public static class StructureHelper
    {
        public static Expression<Func<EventInfo, bool>> IsPublicEvent
        {
            get { return info => info.AddMethod.IsPublic && info.RemoveMethod.IsPublic; }
        }

        public static EventInfo GetEventInfo<TInstance>(string name)
        {
            return typeof(TInstance).GetEvent(name);
        }

        public static Expression<Func<PropertyInfo, bool>> IsPublicProperty
        {
            get { return info => info.GetMethod.IsPublic && info.SetMethod.IsPublic; }
        }

        public static PropertyInfo GetIndexProperty<TInstance>()
        {
            // Based on https://stackoverflow.com/questions/1347936/identifying-a-custom-indexer-using-reflection-in-c-sharp
            foreach (PropertyInfo info in typeof(TInstance).GetAllMembers().OfType<PropertyInfo>())
            {
                if (info.GetIndexParameters().Length > 0)
                {
                    return info;
                }
            }
            throw new ArgumentException($"{typeof(TInstance).Name} does not contain an index property");
        }
    }
}
