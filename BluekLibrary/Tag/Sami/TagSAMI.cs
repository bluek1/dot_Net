using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluekLibrary.Tag.Sami
{
    [TagNameAttr("SAMI")]
    class TagSAMI : TagBase
    {
        public TagSAMI()
        {

        }

        

        public override bool Parse(string str)
        {
            return true;
        }
    }
}
