using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluekLibrary.DB {

    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class AttriDbValue : Attribute {
        public bool IsSelect;
        public bool IsKey;
        public bool IsJoinKey;
        public bool IsParameter;
        public String Name;
        public AttriDbValue(String Name = null, bool IsSelect = true, bool IsKey = false, bool IsJoinKey = false, bool IsParameter = false) {
            this.Name = Name;
            this.IsSelect = IsSelect;
            this.IsKey = IsKey;
            this.IsJoinKey = IsJoinKey;
            this.IsParameter = IsParameter;
        }

    }

}

