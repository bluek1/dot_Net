using bluekFramework.DB.RowItem;
using BluekLibrary.DB;
using System.Collections.Generic;

namespace bluekFramework.DB.Table {
    public class ThumbnailM : DbTable<ThumbnailMRowItem> {
        AllDbConnection mCon;
        public ThumbnailM(AllDbConnection con) : base(con) {
            mCon = con;
        }

        public void SelectLock() {
            lock (mCon.SqlLock) {
                mDr = this.selectAll();
                this.ReadToDataReader();
                mDr.Close();
            }
        }

        public List<ThumbnailMRowItem> GetSelectItemLock() {
            List<ThumbnailMRowItem> result = null;
            lock (mCon.SqlLock) {
                mDr = this.selectAll();
                result=this.ReadToDataReader();
                mDr.Close();
            }
            return result;
        }
    }
}
