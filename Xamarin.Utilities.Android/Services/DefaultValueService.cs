using System;
using Android.Content;
using Xamarin.Utilities.Core.Services;

namespace Xamarin.Utilities.Android.Services
{
    public class DefaultValueService : IDefaultValueService
    {
        private readonly ISharedPreferences _preferences;

        public DefaultValueService(Context androidContext)
        {
            _preferences = androidContext.GetSharedPreferences("default_values", FileCreationMode.Private);
        }

        public T Get<T>(string key)
        {
            if (typeof (T) == typeof (int))
                return (T)(object)_preferences.GetInt(key, 0);
            if (typeof (T) == typeof (string))
                return (T)(object)_preferences.GetString(key, null);
            if (typeof (T) == typeof (bool))
                return (T) (object) _preferences.GetBoolean(key, false);
            throw new NotSupportedException();
        }

        public bool TryGet<T>(string key, out T value)
        {
            try
            {
                value = Get<T>(key);
                return true;
            }
            catch (Exception)
            {
                value = default(T);
                return false;
            }
        }

        public void Set(string key, object value)
        {
            using (var e = _preferences.Edit())
            {
                if (value == null)
                    e.Remove(key);
                else if (value is string)
                    e.PutString(key, value as string);
                else if (value is int)
                    e.PutInt(key, (int)value);
                else if (value is long)
                    e.PutLong(key, (long) value);
                else if (value is bool)
                    e.PutBoolean(key, (bool) value);
                e.Apply();
            }
        }
    }
}