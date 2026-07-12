using System.Reflection;

namespace SarcWrapper.SarcTypes
{
    public class SarcNameAttribute : SarcAttribute<string>
    {
        public SarcNameAttribute(string value) : base(value) { }
    }

    public class SarcExtensionAttribute : SarcAttribute<string>
    {
        public SarcExtensionAttribute(string value) : base(value) { }
    }

    public class SarcInsideFolderAttribute : SarcAttribute<bool>
    {
        public SarcInsideFolderAttribute(bool value) : base(value) { }
    }

    public static class SarcAttribute
    {
        public enum Attributes
        {
            Name,
            Extension,
            InsideFolder,
            FileType
        }

        public static T GetAttribute<Attribute, T>(Attributes attribute)
        {
            return GetAttribute<T>(typeof(Attribute), attribute);
        }

        public static T GetAttribute<T>(Type type, Attributes attribute)
        {
            Type attributeType = null;

            switch (attribute)
            {
                case Attributes.Name:
                    attributeType = typeof(SarcNameAttribute);
                    break;
                case Attributes.Extension:
                    attributeType = typeof(SarcExtensionAttribute);
                    break;
                case Attributes.InsideFolder:
                    attributeType = typeof(SarcInsideFolderAttribute);
                    break;
            }

            return ((SarcAttribute<T>)type.GetCustomAttribute(attributeType, false)).Value;
        }
    }

    public class SarcAttribute<T> : Attribute
    {
        public T Value { get; private set; }
        public SarcAttribute(T value)
        {
            Value = value;
        }
    }
}
