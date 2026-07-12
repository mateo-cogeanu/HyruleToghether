using SarcWrapper.Helper;
using System.Reflection;

namespace SarcWrapper.SarcTypes
{
    public class SarcObject : ISarcObject 
    {
        public SarcObject Parent { get; set; }

        public void SetParent(SarcObject parent) { this.Parent = parent; }

        public void LoadParent()
        {
            if(Parent == null)
            {
                this.LoadChildren();
            }
            else
            {
                Parent.LoadParent();
            }
        }

        public virtual void LoadChildren() { }
    }

    public interface ISarcObject
    {
        public void SetParent(SarcObject parent);
        public void LoadParent();
    }
}
