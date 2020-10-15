using bluekFramework.DB;
using bluekFramework.DB.RowItem;
using bluekFramework.DB.Table;
using BluekLibrary.DB;
using System;
using System.Windows.Forms;

namespace DownWatch {
    public partial class frmCreateItremView : Form {
       

        public frmCreateItremView() {
            InitializeComponent();
        }

        private void frmCreateItremView_Load(object sender, EventArgs e) {
            AllDbConnection con = AllDbConnection.GetConnection();
            FileMRowItem row = new FileMRowItem();
            row.DeleteFlag.Value = "0";
            FileM file = new FileM(con);
            ContentM cont = new ContentM(con);
            

            file.WhereSelect(row, row.UpdateTime.DESC);
            ContentMRowItem content = new ContentMRowItem();

            drFileList.DataSource = file.Rows;
            (this.Owner as frmMain).StopDelFileWatch();

        }

        private void frmCreateItremView_FormClosed(object sender, FormClosedEventArgs e) {
            (this.Owner as frmMain).StopDelFileWatch();
        }

        private void drFileList_CellEndEdit(object sender, DataGridViewCellEventArgs e) {

        }

        private void drFileList_CellClick(object sender, DataGridViewCellEventArgs e) {
            
        }

        private void drFileList_CellContentClick(object sender, DataGridViewCellEventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {
            //video.Play();
        }
    }
}
