using bluekFramework.DB.RowItem;
using BluekLibrary.DB;
using System;
using System.Collections.Generic;

namespace bluekFramework.DB.Table {
    public class FileM: DbTable<FileMRowItem> {
        AllDbConnection mCon;
        public FileM(AllDbConnection con) : base(con) {
            mCon = con;
        }

        public void SelectLock() {
            lock (mCon.SqlLock) {
                mDr = this.selectAll();
                this.ReadToDataReader();
                mDr.Close();
            }
        }

        public void WhereSelectLock(FileMRowItem cond) {
            lock (mCon.SqlLock) {
                mDr = this.selectWhere(cond);
                this.ReadToDataReader();
                mDr.Close();
            }
        }

        public void WhereSelect(FileMRowItem cond) {
            mDr = cond != null? this.selectWhere(cond): this.selectAll();
            this.ReadToDataReader();
            mDr.Close();
        }

        public void WhereSelect(String cond,String OrderBy) {
            mDr = cond != null ? this.selectWhere(cond, OrderBy) : this.selectAll();
            this.ReadToDataReader();
            mDr.Close();
        }
        public void WhereSelect(FileMRowItem cond ,OrderByClass order) {
            mDr = cond != null ? this.selectWhere(cond, order) : this.selectAll(order);
            this.ReadToDataReader();
            mDr.Close();
        }


        public List<FileMRowItem> GetSelectItemLock() {
            List<FileMRowItem> result = null;
            lock (mCon.SqlLock) {
                mDr = this.selectAll();
                result = this.ReadToDataReader();
                mDr.Close();
            }
            return result;
        }
    }
}
