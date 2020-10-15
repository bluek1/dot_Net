using BluekLibrary.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace bluekFramework.DB.RowItem {
    public class ThumbnailMRowItem : DbRowItem {
        public DbValue ThumbnailID { set; get; } = DbValue.ValuleNull;
        public DbValue ThumbnailNO { set; get; } = DbValue.ValuleNull;
        public DbValue KeyWordPatternId { set; get; } = DbValue.ValuleNull;
        [AttriDbValue(IsParameter =true)]
        public DbValue picData { set; get; } = DbValue.ValuleNull;
        public DbValue DeleteFlag { private set; get; } = DbValue.ValuleNull;
        public DbValue UpdateTime { private set; get; } = DbValue.ValuleNull;
        public ThumbnailMRowItem() {

        }
        public ThumbnailMRowItem(IDataReader rd) : base(rd) {

        }

    }
}
