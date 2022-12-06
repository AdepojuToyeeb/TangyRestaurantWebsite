using System;
namespace TangyRestaurantWebsite.Extensions
{
    public static class ReflectionExtension
    {
        public static string GetPropertyValue<T>(this T item, string propertyName)
        {
            var property = item.GetType().GetProperty(propertyName)?.GetValue(item, null).ToString();

            return property;
        }
    }
}
