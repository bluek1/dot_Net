using System.Data;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using BluekLibrary.DB.SQLConst;
using System.Threading;

namespace BluekLibrary.DB {
    public class AllDbConnection : IDbConnection {
        //커넥션 정보
        private IDbConnection iCon;
        //커넥션 인스턴스
        private static AllDbConnection mCon;

        public object SqlLock { get; set; } = new object();

        private IDbTransaction mTran;

        public IDbTransaction Transaction {  get => mTran; }

        private bool lockFlag;

        public static AllDbConnection GetConnection(ConnectStringBuilder sql) {
            if (mCon == null) {
                mCon = new AllDbConnection(sql);
            }
            return mCon;
        }

        public static AllDbConnection GetConnection() {
            if (mCon == null) {
                throw new System.Exception("케넥션 이 생셍되지 않았습니다.");
            }
            return mCon;
        }

        public ISqlKeyWord GetKeyWord { private set; get; }

        public AllDbConnection(ConnectStringBuilder sql) {
            lockFlag = false;
            this.ConnectStringBuilder = sql;
            switch (sql.Type) {
                case CreateConnectType.MS_Sql:
                    iCon = new SqlConnection(sql.CreateString());
                    GetKeyWord = new MysqlKeyWord();
                    break;
                case CreateConnectType.My_Sql:
                    iCon = new MySqlConnection(sql.CreateString());
                    GetKeyWord = new MysqlKeyWord();
                    break;
                case CreateConnectType.Oracle:
                    iCon = new OracleConnection(sql.CreateString());
                    break;
                case CreateConnectType.Ole_db:
                    // ???
                    iCon = new OleDbConnection(sql.CreateString());
                    break;
            }
            
        }

        public ConnectStringBuilder ConnectStringBuilder { private set; get; }

        #region 인터페이스 제정의

        public string ConnectionString { get => iCon.ConnectionString; set => iCon.ConnectionString = value; }

        public int ConnectionTimeout => iCon.ConnectionTimeout;

        public string Database => iCon.Database;

        public ConnectionState State => iCon.State;

        public IDbTransaction BeginTransaction() {
            return iCon.BeginTransaction();
        }

        public void StartTransaction() {
            mTran = iCon.BeginTransaction();
        }

        public void RollBackTransaction() {
            if (mTran!=null) mTran.Rollback();
        }

        public void CommitTransaction() {
            if (mTran != null) mTran.Commit();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il) {
            return iCon.BeginTransaction(il);
        }

        public void ChangeDatabase(string databaseName) {
            iCon.ChangeDatabase(databaseName);
        }

        public void Close() {
            iCon.Close();

        }

        public IDbCommand CreateCommand() {
            return iCon.CreateCommand();
        }

        public void Dispose() {
            iCon.Dispose();
        }

        public void Open() {
            iCon.Open();
        }

        #endregion
    }
}
