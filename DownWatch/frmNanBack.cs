using bluekFramework.Common;
using bluekFramework.DB.RowItem;
using bluekFramework.DB.Table;
using BluekLibrary.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DownWatch {
    public partial class frmNanBack : Form {
        private delegate void progEventProcDelegate(int pos, int max);
        private progEventProcDelegate ProgCopyProcHandle;
        private progEventProcDelegate BackupProcHandle;
        private String backPath = ConfigurationManager.AppSettings["BackDir"];
        private List<FileMRowItem> copyList = new List<FileMRowItem>();
        private Task task;
        public bool EndFlag;
        public frmNanBack() {
            InitializeComponent();
        }

        private void frmNanBack_Load(object sender, EventArgs e) {
            AllDbConnection con = AllDbConnection.GetConnection();
            FileM file = new FileM(con);
            (this.Owner as frmMain).StopDelFileWatch();
            file.WhereSelect("FileUpdateTime > '2020/05/01' and DeleteFlag='0' AND  (BackPath='' OR BackPath IS NULL ) ", "FileUpdateTime");
            copyList = file.Rows;
            ProgCopyProcHandle = new progEventProcDelegate(progFileCopyProcEvent);
            BackupProcHandle = new progEventProcDelegate(BackupProcEvent);
            task = new Task(() => Work_DoWork());
            task.Start();
            EndFlag = false;
            MessageBox.Show("처리 시작"+ copyList.Count.ToString());
        }

        private void Work_DoWork() {
            int cnt = 0;
            int max = copyList.Count;
            AllDbConnection con = AllDbConnection.GetConnection();
            FileM file = new FileM(con);

            try {
                foreach (FileMRowItem row in copyList) {
                    FileMRowItem key = null;
                    FileMRowItem data = null;
                    try {
                        cnt++;
                        key = new FileMRowItem();
                        DateTime update = DateTime.Parse(row.FileUpdateTime.Value.ToString());
                        String backCopyPath = Path.Combine(backPath, String.Format("{0}\\{1}\\{2}", update.Year, update.Month, update.Day));
                        String tPath = Path.Combine(backCopyPath, row.ExpFullName.Value.ToString());
                        String sPath = Path.Combine(row.TmpPath.Value.ToString(), row.ExpFullName.Value.ToString());
                        key.FileID.Value = row.FileID.Value;
                        CommonUtil.FileCopy(sPath, tPath, copyProgEvent);
                        data = new FileMRowItem();
                        data.BackPath.Value = backCopyPath.Replace("\\", "\\\\");
                        file.UpdateWhere(data, key);
                        if (EndFlag) break;
                        File.Delete(sPath);
                        this.Invoke(BackupProcHandle, new object[] { cnt, max });
                    } catch (IOException e) {
                        if (e.Message == "COPY") {
                            if (key != null) {
                                file.DeleteFlagUpdate(key);
                            }
                        }
                    }

                }

            } catch (Exception e) {
                throw e;

            }
        }

        private void frmNanBack_FormClosed(object sender, FormClosedEventArgs e) {
            (this.Owner as frmMain).StopDelFileWatch();
            EndFlag = true;
            
        }

        private void copyProgEvent(int pos, int max) {
            this.Invoke(ProgCopyProcHandle, new object[] { pos, max });
        }
        private void progFileCopyProcEvent(int pos, int max) {
            progFileCopy.Maximum = max;
            progFileCopy.Value = pos;
            label2.Text = String.Format("진행률:{0}MByte / {1}MByte", pos, max);
        }

        private void BackupProcEvent(int pos, int max) {
            progBackUp.Maximum = max;
            progBackUp.Value = pos;
            label1.Text = String.Format("백업진행도:{0}개 / {1}개 완료", pos, max);
        }
    }


}
