using bluekFramework.Common;
using bluekFramework.DB.RowItem;
using bluekFramework.DB.Table;
using bluekFramework.Struct;
using BluekLibrary.DB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DownWatch {
    public partial class frmMain : Form {
        private String tempPath = ConfigurationManager.AppSettings["TempDir"];
        private String watchPath = ConfigurationManager.AppSettings["watchDir"];
        private ConnectStringBuilder conStr;
        private AllDbConnection mCon = null;
        private frmLog log = null;
        private FileSystemWatcher fw = null;
        private FileSystemWatcher delFw = null;
        private const int maxThreadCount =3;
        private int ThreadCount = 0;
        private List<Task> taskList=new List<Task>();
        #region ======내부 이벤트=====
        public frmMain() {
            InitializeComponent();
        }

        private void mnuEnd_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e) {
            mainIcon.Icon = Icon.FromHandle(Properties.Resources.start.GetHicon());
            String nasIp = ConfigurationManager.AppSettings["NAS_IP"];
            String nasMac = ConfigurationManager.AppSettings["NAS_MAC"];
            int port = int.Parse(ConfigurationManager.AppSettings["NAS_Port"]);
            String[] macs = nasMac.Split('-');
            if (macs.Length == 6) {
                byte[] mac = { Convert.ToByte(macs[0], 16)
                    , Convert.ToByte(macs[1], 16)
                    , Convert.ToByte(macs[2], 16)
                    , Convert.ToByte(macs[3], 16)
                    , Convert.ToByte(macs[4], 16)
                    , Convert.ToByte(macs[5], 16) };
                bool resutl = false;
                //나스 상태 확인
                resutl = CommonUtil.Ping(nasIp, 5000);

                if (!resutl) {
                    mainIcon.Icon = Icon.FromHandle(Properties.Resources.boot.GetHicon());
                    WriteTextAppendText("NAS기동처리 시작");
                    while (!resutl) {
                        CommonUtil.WakeUp(mac);
                        Thread.Sleep(5000);
                        resutl = CommonUtil.Ping(nasIp, port);
                    }
                    WriteTextAppendText("NAS기동 기동 확인 ");
                }
            }

            mainIcon.Icon = Icon.FromHandle(Properties.Resources.nas.GetHicon());
            //파일 감시 시작
            WriteTextAppendText("다운로드 파일 폴더감시 시작");
            this.StartFileWatch();

            Thread T = new Thread(new ThreadStart(ThreadProc));
            T.Start();
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e) {
            this.StopFileWatch();
        }

        #endregion

        /// <summary>
        /// 폴더 감시시작
        /// </summary>
        private void StartFileWatch() {
            conStr = ConnectStringBuilder.GetConfigFile();
            mCon = AllDbConnection.GetConnection(conStr);
            mCon.Open();
            mainIcon.Text = "감시";
            fw = new FileSystemWatcher(watchPath);
            delFw = new FileSystemWatcher(tempPath);

            fw.NotifyFilter = NotifyFilters.FileName;
            delFw.NotifyFilter = NotifyFilters.FileName;
            fw.Changed += Fw_Changed;
            fw.Created += Fw_Changed;
            fw.EnableRaisingEvents = true;
            this.StartDelFileWatch();
        }


        /// <summary>
        /// 파일 감시 정지
        /// </summary>
        private void StopFileWatch() {
            mainIcon.Text = "중지";
            this.StopDelFileWatch();
            fw.EnableRaisingEvents = false;
            fw.Changed -= Fw_Changed;
            fw.Created -= Fw_Changed;
            mainIcon.Dispose();
            fw.Dispose();
            mCon.Close();
            mCon.Dispose();
        }

        public void StopDelFileWatch() {
            delFw.EnableRaisingEvents = false;
            delFw.IncludeSubdirectories = false;
            delFw.Deleted -= DelFw_Deleted;
        }

        public void StartDelFileWatch() {
            delFw.IncludeSubdirectories = true;
            delFw.EnableRaisingEvents = true;
            delFw.Deleted += DelFw_Deleted;
        }


        /// <summary>
        /// 스레드 처리 변경 확인후 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Fw_Changed(object sender, FileSystemEventArgs e) {
            Task task = new Task(() => FileMove(e.FullPath, e.Name));
            taskList.Add(task);
        }

        /// <summary>
        /// 스레드 처리 삭제 확인후 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelFw_Deleted(object sender, FileSystemEventArgs e) {
            WriteTextAppendText("삭제처리 발견");
            FileInfo fi = new FileInfo(e.FullPath);
            WriteTextAppendText(String.Format("파일명:{0}", fi.FullName));
            FileM file = new FileM(mCon);
            FileMRowItem key = new FileMRowItem();
            FileMRowItem updateData = new FileMRowItem();
            fi.DirectoryName.Replace("\\", "\\\\");
            key.ExpFullName.Value = fi.Name;
            key.TmpPath.Value = fi.DirectoryName.Replace("\\", "\\\\");
            file.DeleteFlagUpdate(key, true);
            WriteTextAppendText("논리삭제 완료!!");

        }

        /// <summary>
        /// 비동기 파일 복사
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        private bool FileMove(String path, String fileName) {
            WriteTextAppendText("처러 개시:" + fileName+"->"+ ThreadCount.ToString());

            
            Console.WriteLine("Down Done!:{0}", path);
           

            FileInfo fi = new FileInfo(path);
            
            while (CommonUtil.IsFileLocked(path)) {
                Thread.Sleep(1000);
            }
            String copyToPath = String.Format("{0}\\{1:D2}\\{2:D2}", tempPath, fi.CreationTime.Month, fi.CreationTime.Day % 10);
            Console.WriteLine("이동대상 경로:{0}",copyToPath);
            WriteTextAppendText("이동대상 경로:"+ copyToPath);
            if (!Directory.Exists(copyToPath)) {
                Directory.CreateDirectory(copyToPath);
            }
            try {
                NRceoVideoInfo info=CommonUtil.GetVedioInfo(path);
                String hashCode = CommonUtil.ComputeMD5Hash(path);

                //파일 정보 로드
                FileM file = new FileM(mCon);
                FileMRowItem key = new FileMRowItem();
                key.Hash.Value = hashCode;
                file.WhereSelectLock(key);
                lock (mCon.SqlLock) {
                    if (file.Rows.Count > 0) {
                        WriteTextAppendText("중복파일 발견:" + fileName);
                        File.Delete(path);
                        //FileMRowItem row = new FileMRowItem();

                        //row.DeleteFlag.Value = "0";
                        ////row.UpdateTime = DbValue.ValuleNull;
                        //row.FileUpdateTime = DbValue.ValuleNull;
                        //row.Hash = DbValue.ValuleNull;
                        //row.ExpFullName = DbValue.ValuleNull; ;
                        //row.ExpName = DbValue.ValuleNull; ;
                        //row.FileName = DbValue.ValuleNull; ;
                        //row.TmpPath = DbValue.ValuleNull; ;
                        //row.FileUpdateTime = DbValue.ValuleNull; ;
                        //row.Size = DbValue.ValuleNull; ;
                        //row.Type = DbValue.ValuleNull; ;
                        //file.UpdateWhere(row, key);

                        return true;
                    }
                }
                String TgaFullName = String.Format("{0}\\{1}", copyToPath, fileName);
                ContentM cont = new ContentM(mCon);
                ContentMRowItem content = new ContentMRowItem();

                WriteTextAppendText("복사 처리 시작:" + fileName);
                File.Copy(path, TgaFullName, true);
                WriteTextAppendText("원본 삭제" );
                File.Delete(path);

                //DB등록 시작                
                fi = new FileInfo(TgaFullName);
                FileMRowItem moveFile = new FileMRowItem();
                // 파일 정보 추가
                lock (mCon.SqlLock) {
                    mCon.StartTransaction();
                    moveFile.Hash.Value = hashCode;
                    moveFile.ExpFullName.Value = fileName;
                    moveFile.ExpName.Value = fi.Extension;
                    moveFile.FileName.Value = fi.Name.Replace(fi.Extension, "");
                    moveFile.TmpPath.Value = fi.DirectoryName.Replace("\\", "\\\\");
                    moveFile.FileUpdateTime.Value = fi.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss");
                    moveFile.Size.Value = fi.Length.ToString();
                    
                    file.Insert(moveFile, true);
                    file.WhereSelect(key);
                    if (file.Rows.Count > 0) {
                        // 컨텐츠 정보 추가
                        content.Title = moveFile.FileName;
                        content.Time.Value = info.PlayTime;
                        content.ScreenX.Value = info.Width;
                        content.ScreenY.Value = info.Height;
                        content.FileID.Value = file.Rows[0].FileID.Value;
                        cont.Insert(content, true);
                        WriteTextAppendText("DB등록 완료:" + TgaFullName);
                    }
                    mCon.CommitTransaction();
                }
            } catch(Exception e) {
                WriteTextAppendText("에러:"+e.Message);
                mCon.RollBackTransaction();
                return false;
            }
            ThreadCount--;
            WriteTextAppendText("스레드 할당해제:"+ ThreadCount);
            return true;
        }
        /// <summary>
        /// 메인 잡 스래드
        /// </summary>
        private void ThreadProc() {
            WriteTextAppendText("잡스래드 개시");
            while (this!=null) {
                ThreadCount = 0;
                if (taskList.Count > 0) {
                    foreach(Task t in taskList) {
                        if (t.Status== TaskStatus.Running) {
                            ThreadCount++;
                        }
                    }
                    foreach (Task t in taskList) {
                        if ((t.Status == TaskStatus.WaitingForActivation||
                            t.Status== TaskStatus.Created )&& ThreadCount<=maxThreadCount) {
                            t.Start();
                            ThreadCount++;
                        }
                    }
                    for (int i = taskList.Count-1; i >= 0; i--) {
                        if (taskList[i].Status== TaskStatus.RanToCompletion) {
                            taskList.RemoveAt(i);
                        }
                    }

                }
                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// 로그 쓰기 (멀티스레드)
        /// </summary>
        public void WriteTextAppendText(string text) {
            Common.logList.Add(text);
            if (log != null&&log.Visible) {
                log.DrawLog(text);
            }
        }

        /// <summary>
        /// 로그창 열기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuLog_Click(object sender, EventArgs e) {
            log = new frmLog();
            log.Show();
        }

        private void nmuInitDataSet_Click(object sender, EventArgs e) {
            frmCreateItremView frm = new frmCreateItremView();
            frm.Owner = this;
            frm.Show();
                
        }

        private void nmuBackup_Click(object sender, EventArgs e) {
                        frmNanBack frm = new frmNanBack();
            frm.Show(this);
            
        }
    }
}
