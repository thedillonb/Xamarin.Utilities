using System;
using MonoTouch.Foundation;
using Xamarin.Utilities.Core.Services;

namespace Xamarin.Utilities.Services
{
    public class DefaultValueService : IDefaultValueService
    {
        public static NSUserDefaults Defaults = NSUserDefaults.StandardUserDefaults;

        public T Get<T>(string key)
        {
            if (typeof(T) == typeof(int))
                return (T)(object)Defaults.IntForKey(key);
            if (typeof(T) == typeof(bool))
                return (T)(object)Defaults.BoolForKey(key);
            throw new Exception("Key does not exist in Default database.");
        }

        public bool TryGet<T>(string key, out T value)
        {
            try
            {
                value = Get<T>(key);
                return true;
            }
            catch
            {
                value = default(T);
                return false;
            }
        }

        public void Set(string key, object value)
        {
            if (value == null)
                Defaults.RemoveObject(key);
            else if (value is int)
                Defaults.SetInt((int)value, key);
            else if (value is bool)
                Defaults.SetBool((bool)value, key);
            Defaults.Synchronize();
        }
    }
}
