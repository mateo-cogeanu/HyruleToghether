using System.Reflection;

namespace SarcWrapper.SarcTypes
{
    public class AampIsHash : AampAttribute<bool>
    {
        public AampIsHash(bool value) : base(value) { }
    }

    public class AampAttributes
    {
        public enum Attributes
        {
            IsHash
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
                case Attributes.IsHash:
                    attributeType = typeof(SarcNameAttribute);
                    break;
            }

            return ((SarcAttribute<T>)type.GetCustomAttribute(attributeType, false)).Value;
        }
    }

    public class AampAttribute<T> : Attribute
    {
        public T Value { get; private set; }
        public AampAttribute(T value)
        {
            Value = value;
        }
    }
}
