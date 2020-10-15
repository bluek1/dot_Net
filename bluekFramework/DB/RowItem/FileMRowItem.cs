using BluekLibrary.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace bluekFramework.DB.RowItem {
    public class FileMRowItem : DbRowItem {
        public DbValue FileID { set; get; } = DbValue.ValuleNull;
        public DbValue FileName { set; get; } = DbValue.ValuleNull;
        public DbValue LocalPath { set; get; } = DbValue.ValuleNull;
        public DbValue BackPath { set; get; } = DbValue.ValuleNull;
        public DbValue TmpPath { set; get; } = DbValue.ValuleNull;
        public DbValue Type { set; get; } = DbValue.ValuleNull;
        public DbValue ExpName { set; get; } = DbValue.ValuleNull;
        public DbValue ExpFullName { set; get; } = DbValue.ValuleNull;
        public DbValue Size { set; get; } = DbValue.ValuleNull;
        public DbValue FileUpdateTime { set; get; } = DbValue.ValuleNull;
        public DbValue Hash { set; get; } = DbValue.ValuleNull;
        public DbValue DeleteFlag { set; get; } = DbValue.ValuleNull;
        public DbValue UpdateTime { private set; get; } = DbValue.ValuleNull;
        public FileMRowItem() {

        }
        public FileMRowItem(IDataReader rd) : base(rd) {

        }

    }
}
 