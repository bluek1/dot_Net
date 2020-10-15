using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluekLibrary.Tag
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TagNameAttr : Attribute
    {
        public string TagName { private set; get; }
        public TagNameAttr(string name)
        {
            TagName = name;
        }
    }
}
