using MongoDB.Bson.Serialization;
using Newtonsoft.Json;

namespace TDStore.Helpers
{
    public static class SessionHelper
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            System.Diagnostics.Debug.WriteLine(value);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
