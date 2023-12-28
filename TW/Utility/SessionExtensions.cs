using System.Text;
using Newtonsoft.Json;

namespace TW.Utility
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.Get(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(value));
        }


    }
}
