using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluekLibrary.DB {
    public class DbValue  {
        object value;
        String name;
        bool selectVisible;
        bool keyFlag;
        bool keyJoin;
        bool isParameter;

        public DbValue(String val) {
            value = val;
            selectVisible = true;
            keyFlag = false;
            keyJoin = false;
            isParameter = false;
        }

        public DbValue(AttriDbValue attri) {
            value = null;
            selectVisible = attri.IsSelect;
            keyFlag = attri.IsKey;
            keyJoin = attri.IsJoinKey;
            name = attri.Name;
            isParameter = attri.IsParameter;
        }

        public DbValue(String val, AttriDbValue attri) {
            value = val;
            selectVisible = attri.IsSelect;
            keyFlag = attri.IsKey;
            keyJoin = attri.IsJoinKey;
            name = attri.Name;
            isParameter = attri.IsParameter;
        }

        public static DbValue ValuleNull => new DbValue(null as String);

        public override string ToString() {
            if (value == null) {
                return null;
            } else {
                if (isParameter) {
                    return "@"+name;
                } else {
                    return value.ToString();
                }
                
            }
        }

        public object Value {
            set {
                this.value = value;
            }
            get {
                return this.value;
            }
        } 
        public asc ASC =>new asc(name);

        public desc DESC=> new desc(name);

        public bool SelectItemVisible => selectVisible;

        public bool IsParameter => this.isParameter;

        public bool IsNull {
            get { 
             return value == null;
            }
        }

        public bool IsKey() {
            return keyFlag;
        }

        public bool IsJoinKey() {
            return keyJoin;
        }

        public void SetName(String str) {
            this.name = str;
        }

        public IDataParameter GetParameter(ConnectStringBuilder sqlString) {
            IDataParameter rtn = null ;
            String name = String.Format("@{0}", this.name);
            switch (sqlString.Type) {
                case CreateConnectType.MS_Sql:
                    rtn = new SqlParameter(name, this.value);
                    break;
                case CreateConnectType.My_Sql:
                    rtn = new MySqlParameter(name, this.value);
                    break;
                case CreateConnectType.Oracle:
                    rtn = new OracleParameter(name, this.value);
                    break;
                case CreateConnectType.Ole_db:
                    // ???
                    rtn = new OleDbParameter(name, this.value);
                    break;
            }
            return rtn;
        }

    }
}
