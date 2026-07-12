using SarcWrapper.SarcTypes;
using System.Reflection;

namespace SarcWrapper.Helper
{
    public static class FieldInfoExtensions
    {
        public static bool IsListOfSarcFiles(this FieldInfo field)
        {
            return field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(SarcFileList<,>) && typeof(ISarcFile).IsAssignableFrom(field.FieldType.GetGenericArguments()[0]);
        }

        public static bool IsSarcFolder(this FieldInfo field)
        {
            return typeof(ISarcFolder).IsAssignableFrom(field.FieldType);
        }

        public static bool IsSarcFile(this FieldInfo field)
        {
            return typeof(ISarcFile).IsAssignableFrom(field.FieldType);
        }

        public static bool IsListOfSarcFiles(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(SarcFileList<,>) && typeof(ISarcFile).IsAssignableFrom(type.GetGenericArguments()[0]);
        }

        public static bool IsSarcFolder(this Type type)
        {
            return typeof(ISarcFolder).IsAssignableFrom(type);
        }

        public static bool IsSarcFile(this Type type)
        {
            return typeof(ISarcFile).IsAssignableFrom(type);
        }
    }
}
