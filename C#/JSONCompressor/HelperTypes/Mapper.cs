using System.Reflection;

namespace BOTWM.Server.HelperTypes
{
    static public class AutoMapperExtensions
    {
        static public void Map<T>(this T Dst, object Src)
        {
            foreach (FieldInfo field in typeof(T).GetFields())
            {
                if (!Src.GetType().GetFields().Any(fld => fld.Name == field.Name && fld.FieldType == field.FieldType))
                    continue;

                FieldInfo SrcField = Src.GetType().GetFields().Where(fld => fld.Name == field.Name && fld.FieldType == field.FieldType).First();

                field.SetValue(Dst, SrcField.GetValue(Src));
            }
        }
    }
}
