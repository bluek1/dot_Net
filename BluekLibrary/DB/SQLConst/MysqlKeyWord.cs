using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluekLibrary.DB.SQLConst {
    public class MysqlKeyWord : ISqlKeyWord {
        public string getSystemDate() {
            return "NOW()";
        }
    }
}
