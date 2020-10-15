using bluekFramework.DB.RowItem;
using BluekLibrary.DB;
using System.Collections.Generic;

namespace bluekFramework.DB.Table {
    public class ContentM : DbTable<ContentMRowItem> {
        AllDbConnection mCon;
        public ContentM(AllDbConnection con) : base(con) {
            mCon = con;
        }

        public void SelectLock() {
            lock (mCon.SqlLock) {
                mDr = this.selectAll();
                this.ReadToDataReader();
                mDr.Close();
            }
        }

        public List<ContentMRowItem> GetSelectItemLock() {
            List<ContentMRowItem> result = null;
            lock (mCon.SqlLock) {
                mDr = this.selectAll();
                result=this.ReadToDataReader();
                mDr.Close();
            }
            return result;
        }
    }
}
