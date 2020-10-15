using BluekLibrary.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace bluekFramework.DB.RowItem {
    public class ContentMRowItem : DbRowItem {
        public DbValue MoveID { set; get; } = DbValue.ValuleNull;
        public DbValue FileID { set; get; } = DbValue.ValuleNull;
        public DbValue Title { set; get; } = DbValue.ValuleNull;
        public DbValue PictureListId { set; get; } = DbValue.ValuleNull;
        public DbValue KeyWordListId { set; get; } = DbValue.ValuleNull;
        public DbValue ScreenX { set; get; } = DbValue.ValuleNull;
        public DbValue ScreenY { set; get; } = DbValue.ValuleNull;
        public DbValue Time { set; get; } = DbValue.ValuleNull;
        public DbValue Rank { set; get; } = DbValue.ValuleNull;
        public DbValue DeleteFlag { private set; get; } = DbValue.ValuleNull;
        public DbValue UpdateTime { private set; get; } = DbValue.ValuleNull;
        public ContentMRowItem() {

        }
        public ContentMRowItem(IDataReader rd) : base(rd) {

        }

    }
}
