using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Reflection;

namespace BluekLibrary.DB
{
    /// <summary>
    /// DB접속 문자열 클래스
    /// </summary>
    public class ConnectStringBuilder {
        [Category("ConfigItem")]
        public String ServerName { set; get; }
        [Category("ConfigItem")]
        public String UserID { set; get; }
        [Category("ConfigItem")]
        public String Password { set; get; }
        [Category("ConfigItem")]
        public String DataBaseName { set; get; }
        [Category("ConfigItem")]
        public CreateConnectType Type { set; get; }
        [Category("ConfigItem")]
        public String Port { set; get; }

        private String fileName;

        public static ConnectStringBuilder GetConfigFile() {
            ConnectStringBuilder rtn = new ConnectStringBuilder();
            CreateConnectType type= CreateConnectType.Oracle;
            NameValueCollection items = (NameValueCollection)ConfigurationManager.GetSection("ConnectStringBuilder");

            foreach (PropertyInfo prop in rtn.GetType().GetProperties()) {
                object att = rtn.GetType().GetProperty(prop.Name).GetCustomAttribute(typeof(CategoryAttribute));
                if (att != null) {
                    rtn.GetType().GetProperty(prop.Name).SetValue(rtn, items[prop.Name]);
                }
            }
            //`데이테베이스 종류
            if (items["DbType"] != null && items["DbType"] != "") {
                Enum.TryParse(items["DbType"],out type);
            }
            rtn.Type = type;
            return rtn;
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="type">입력 형식</param>
        public ConnectStringBuilder() {
        }

        /// <summary>
        /// 생성자(MSSQL,MYSQL용)
        /// </summary>
        /// <param name="server">서버명</param>
        /// <param name="ID">유져명</param>
        /// <param name="pass">암호</param>
        /// <param name="DbName">DB명</param>
        public ConnectStringBuilder(String server,String ID,String pass,String DbName, CreateConnectType type) {
            this.ServerName = server;
            this.UserID = ID;
            this.Password = pass;
            this.DataBaseName = DbName;
            this.Type = type;
        }

        /// <summary>
        /// 생성자(MSSQL,MYSQL용)
        /// </summary>
        /// <param name="server">서버명</param>
        /// <param name="ID">유져명</param>
        /// <param name="pass">암호</param>
        /// <param name="DbName">DB명</param>
        public ConnectStringBuilder(String server, String ID, String pass, String DbName,String port, CreateConnectType type) {
            this.ServerName = server;
            this.UserID = ID;
            this.Password = pass;
            this.DataBaseName = DbName;
            this.Type = type;
            this.Port = port;
        }


        /// <summary>
        /// 생성자(오라쿨용)
        /// </summary>
        /// <param name="ID">유져명</param>
        /// <param name="pass">암호</param>
        /// <param name="DataSource">DB소스명</param>
        public ConnectStringBuilder(String ID, String pass, String DataSource) {
            this.UserID = ID;
            this.Password = pass;
            this.DataBaseName = DataSource;
            this.Type = CreateConnectType.Oracle;
        }
        /// <summary>
        /// 생성자(액세스용)
        /// </summary>
        /// <param name="DbFileName">엑세스 파일패스</param>
        public ConnectStringBuilder(String DbFileName) {
            fileName = DbFileName;
            this.Type = CreateConnectType.Ole_db;
        }

        /// <summary>
        /// 접속 문자열 만들기
        /// </summary>
        /// <param name="type">문자열 형식</param>
        /// <returns></returns>
        public String CreateString() {
            String rtn="";
            switch (this.Type) {
                case CreateConnectType.MS_Sql:
                case CreateConnectType.My_Sql:
                    if (Port == "") {
                        rtn = String.Format("Server={0};Database={1};Uid={2};Pwd={3};", ServerName, DataBaseName, UserID, Password);
                    } else {
                        rtn = String.Format("Server={0};Database={1};Uid={2};Pwd={3};port={4}", ServerName, DataBaseName, UserID, Password, Port);
                    }
                    break;
                case CreateConnectType.Oracle:
                    rtn = String.Format("DATA SOURCE = {0}; User Id = {1}; Password = {2}", DataBaseName, UserID, Password);
                    break;
                case CreateConnectType.Ole_db:
                    rtn = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Source={0}", fileName);
                    break;
            }
            return rtn;
        }
    }
}
