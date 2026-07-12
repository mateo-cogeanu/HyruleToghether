using SarcWrapper.Helper;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SarcWrapper.SarcTypes
{
    public class SarcFileList<T, FileType> : 
        List<T>, ISarcFileList 
        where T : SarcFile<T, FileType>
    {
        public SarcObject Parent { get; set; }

        public void SetParent(SarcObject parent)
        {
            Parent = parent;
        }

        public void LoadParent()
        {
            Parent.LoadParent();
        }

        public T File(string name)
        {
            if(this.Count() == 0)
            {
                if(Parent.GetType().IsSarcFolder())
                {
                    this.LoadParent();
                }
                else if(Parent.GetType().IsSarcFile())
                {
                    Parent.LoadChildren();
                }
            }

            T result = this.Single(item => item.FileName == name);
            result.LoadChildren();

            return result;
        }

        public List<T> Files()
        {
            if (this.Count() == 0)
            {
                if (Parent.GetType().IsSarcFolder())
                {
                    this.LoadParent();
                }
                else if (Parent.GetType().IsSarcFile())
                {
                    Parent.LoadChildren();
                }
            }

            return this;
        }

        public List<ISarcFile> GetFiles()
        {
            return new List<ISarcFile>(this);
        }
    }

    public interface ISarcFileList : IList
    {
        public void SetParent(SarcObject parent);
        public void LoadParent();
        public List<ISarcFile> GetFiles();
    }
}
