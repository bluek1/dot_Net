using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluekLibrary.DB {
    public class AllDbTransaction {
        public AllDbConnection mCom;
        public IDbTransaction mTran;

        public AllDbTransaction(AllDbConnection com) {
            mCom = com;
            mTran = mCom.BeginTransaction();
        }

        public void  Commit() => mTran.Commit();

        public void Rollback() => mTran.Rollback();

        public void Dispose() => mTran.Dispose();

        public int UpdateExecute(DbTable<DbRowItem> table, DbRowItem data, DbRowItem cond) {
            int cnt=table.UpdateWhere(data, cond);
            return cnt;
        }

        //public int InsertExecute(DbTable<DbRowItem> table, DbRowItem data) {

        //}
    }
}
 