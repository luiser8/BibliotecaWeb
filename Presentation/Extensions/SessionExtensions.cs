// Presentation/Extensions/SessionExtensions.cs
using System.Text.Json;

namespace Presentation.Extensions
{
    public static class SessionExtensions
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        // Métodos específicos para tipos comunes
        public static List<string> GetStringList(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? new List<string>() : JsonSerializer.Deserialize<List<string>>(value);
        }

        public static void SetStringList(this ISession session, string key, List<string> value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }
    }
}