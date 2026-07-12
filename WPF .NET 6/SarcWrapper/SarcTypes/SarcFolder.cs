using SarcWrapper.Helper;
using System.Reflection;

namespace SarcWrapper.SarcTypes
{
    public class SarcFolder<T> : SarcObject, ISarcFolder
        where T : SarcFolder<T>
    {

        public SarcFolder()
        {
            foreach (FieldInfo field in typeof(T).GetFields().Where(f => f.IsSarcFolder() || f.IsListOfSarcFiles()))
            {
                field.FieldType.GetMethod("SetParent").Invoke(field.GetValue(this), new object[] { this });
            }
        }

        public override void LoadChildren()
        {
            throw new NotImplementedException("Folders should call LoadChildren(data, folder)");
        }

        public void LoadChildren(Dictionary<string, ArraySegment<byte>> data, string folder = "")
        {
            string folderPath = Path.Combine(folder, SarcAttribute.GetAttribute<T, string>(SarcAttribute.Attributes.Name));

            foreach (FieldInfo field in typeof(T).GetFields().Where(f => f.IsSarcFolder()))
            {
                MethodInfo LoadChildrenMethod = field.FieldType.GetMethods().Single(method => method.Name == "LoadChildren" && method.GetParameters().Count() == 2);
                LoadChildrenMethod.Invoke(field.GetValue(this), new object[] { data.filterByFolder(folderPath), folderPath });
                //field.FieldType.GetMethod("LoadChildren").Invoke(field.GetValue(this), new object[] { data.filterByFolder(folderPath), folderPath });
            }

            foreach (FieldInfo field in typeof(T).GetFields().Where(f => f.IsListOfSarcFiles()))
            {
                Type sarcFileType = field.FieldType.GetGenericArguments()[0];

                var extension = SarcAttribute.GetAttribute<string>(sarcFileType, SarcAttribute.Attributes.Extension);

                ISarcFileList files = field.GetValue(this) as ISarcFileList;
                
                foreach(KeyValuePair<string, ArraySegment<byte>> kvp in data.filterByExtension(extension))
                {
                    object sarcFile = Activator.CreateInstance(sarcFileType, new object[] { kvp.Key, kvp.Value });
                    sarcFileType.GetProperty("Parent").SetValue(sarcFile, this);
                    files.Add(sarcFile);
                }
            }
        }

        public Dictionary<string, ArraySegment<byte>> Save()
        {
            Dictionary<string, ArraySegment<byte>> Files = new Dictionary<string, ArraySegment<byte>>();

            foreach (FieldInfo field in typeof(T).GetFields().Where(f => f.IsSarcFolder()))
            {
                Dictionary<string, ArraySegment<byte>> folderData = (Dictionary<string, ArraySegment<byte>>)field.FieldType.GetMethod("Save").Invoke(field.GetValue(this), new object[] { });
                foreach(var folder in folderData)
                {
                    Files[folder.Key] = folder.Value;
                }
            }

            foreach(FieldInfo field in typeof(T).GetFields().Where(f => f.IsListOfSarcFiles()))
            {
                Type sarcType = field.FieldType.GetGenericArguments()[0];
                Type sarcFileType = field.FieldType.GetGenericArguments()[1];

                List<ISarcFile> files = (List<ISarcFile>)field.FieldType.GetMethod("GetFiles").Invoke(field.GetValue(this), new object[] { });

                foreach(ISarcFile file in files)
                {
                    Tuple<string, ArraySegment<byte>>? SavedFile = file.Save();

                    if(SavedFile != null)
                    {
                        Files[SavedFile.Item1] = SavedFile.Item2;
                    }
                }
            }

            return Files;
        }
    }

    public interface ISarcFolder
    {
        public void LoadChildren(Dictionary<string, ArraySegment<byte>> data, string folder);
        public Dictionary<string, ArraySegment<byte>> Save();
    }
}
