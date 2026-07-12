using CsYaz0;
using Nintendo.Aamp;
using SarcLibrary;
using SarcWrapper.Helper;
using System.Collections;
using System.Reflection;

namespace SarcWrapper.SarcTypes
{
    public class SarcFile<T, FileType> : SarcObject, ISarcFile
        where T : SarcFile<T, FileType>
    {
        public string FileName { get { return System.IO.Path.GetFileNameWithoutExtension(Path); } }
        private string Path { get; set; }
        public ArraySegment<byte> Data { get; private set; }
        private bool NeedsCompression { get; } = SarcAttribute.GetAttribute<T, string>(SarcAttribute.Attributes.Extension).StartsWith(".s");
        private bool IsLoaded { get; set; } = false;
        public FileType LoadedData { get; set; }
        private bool IsCustomized = false;

        public SarcFile(string path) : this()
        {
            Path = path;
        }

        public SarcFile(string path, ArraySegment<byte> data) : this()
        {
            Path = path;
            Data = this.NeedsCompression ? Yaz0.Decompress(data) : data;
        }
        
        private SarcFile()
        {
            foreach (FieldInfo field in typeof(T).GetFields().Where(f => f.IsSarcFolder() || f.IsListOfSarcFiles()))
            {
                field.FieldType.GetMethod("SetParent").Invoke(field.GetValue(this), new object[] { this });
            }
        }

        public override void LoadChildren()
        { 
            if(this.Data == null)
            {
                if(File.Exists(Path))
                {
                    byte[] fileData = File.ReadAllBytes(Path);
                    Data = this.NeedsCompression ? Yaz0.Decompress(fileData) : fileData;
                }
                else
                {
                    this.LoadParent();
                }
            }

            if (typeof(FileType) == typeof(AampFile))
            {
                LoadedData = (FileType)(object)AampFile.FromBinary(this.Data.ToArray());
            }
            else if (typeof(FileType) == typeof(Sarc))
            {
                LoadedData = (FileType)(object)Sarc.FromBinary(this.Data);

                foreach(FieldInfo field in typeof(T).GetFields().Where(f => f.IsSarcFolder()))
                {
                    MethodInfo LoadChildrenMethod = field.FieldType.GetMethods().Single(method => method.Name == "LoadChildren" && method.GetParameters().Count() == 2);
                    LoadChildrenMethod.Invoke(field.GetValue(this), new object[] { LoadedData, "" });
                }

                foreach(FieldInfo field in typeof(T).GetFields().Where(f => f.IsListOfSarcFiles()))
                {
                    Type sarcFileType = field.FieldType.GetGenericArguments()[0];

                    var extension = SarcAttribute.GetAttribute<string>(sarcFileType, SarcAttribute.Attributes.Extension);

                    ISarcFileList files = field.GetValue(this) as ISarcFileList;

                    foreach (KeyValuePair<string, ArraySegment<byte>> kvp in ((Sarc)(object)LoadedData).filterByExtension(extension))
                    {
                        object sarcFile = Activator.CreateInstance(sarcFileType, new object[] { kvp.Key, kvp.Value });
                        sarcFileType.GetProperty("Parent").SetValue(sarcFile, this);
                        files.Add(sarcFile);
                    }
                }
            }

            IsLoaded = true;
        }

        public FileType GetLoadedData()
        {
            if (LoadedData == null)
                this.LoadChildren();

            return this.LoadedData;
        }

        public string GetByteString()
        {
            return string.Join(" ", this.Data.Select(b => b.ToString("X2")));
        }

        public void SetCustomData(byte[] newData)
        {
            this.Data = newData;
            this.IsCustomized = true;
        }

        private void EncodeData()
        {
            if (typeof(FileType) == typeof(AampFile))
            {
                this.Data = ((AampFile)(object)this.LoadedData).ToBinary();
            }
            else if (typeof(FileType) == typeof(Sarc))
            {
                foreach (FieldInfo field in typeof(T).GetFields().Where(f => f.IsSarcFolder()))
                {
                    Dictionary<string, ArraySegment<byte>> folderData = (Dictionary<string, ArraySegment<byte>>)field.FieldType.GetMethod("Save").Invoke(field.GetValue(this), new object[] { });

                    foreach (KeyValuePair<string, ArraySegment<byte>> item in folderData)
                    {
                        ((Sarc)(object)this.LoadedData)[item.Key] = item.Value;
                    }
                }

                foreach (FieldInfo field in typeof(T).GetFields().Where(f => f.IsListOfSarcFiles()))
                {
                    List<ISarcFile> files = (List<ISarcFile>)field.FieldType.GetMethod("GetFiles").Invoke(field.GetValue(this), new object[] { });

                    foreach (ISarcFile file in files)
                    {
                        Tuple<string, ArraySegment<byte>>? SavedFile = file.Save();

                        if (SavedFile != null)
                        {
                            ((Sarc)(object)this.LoadedData)[SavedFile.Item1] = SavedFile.Item2;
                        }
                    }
                }

                using MemoryStream ms = new();
                {
                    ((Sarc)(object)this.LoadedData).Write(ms);
                }

                this.Data = ms.ToArray();
            }
        }

        public Tuple<string, ArraySegment<byte>>? Save(bool saveToFileSystem = false, string outputPath = "")
        {
            if (!this.IsCustomized)
            {
                if (!this.IsLoaded)
                {
                    return null;
                }

                this.EncodeData();
            }

            if(saveToFileSystem)
            {
                if(File.Exists(Path))
                {
                    File.WriteAllBytes(string.IsNullOrEmpty(outputPath) ? Path : outputPath, this.NeedsCompression ? Yaz0.Compress(this.Data).ToArray() : this.Data.ToArray());
                }
                else
                {
                    throw new Exception($"{this.Path} does not exist on the filesystem.");
                }
            }

            this.IsCustomized = false;

            return new Tuple<string, ArraySegment<byte>>(this.Path, this.NeedsCompression ? Yaz0.Compress(this.Data).ToArray() : this.Data);
        }
    }

    public interface ISarcFile
    {
        public void LoadChildren();
        public string GetByteString();
        public void SetCustomData(byte[] newData);
        public Tuple<string, ArraySegment<byte>>? Save(bool saveToFileSystem = false, string outputPath = "");
    }
}
