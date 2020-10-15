namespace DownWatch {
    partial class frmMain {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.mainIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.mainMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.nmuInitDataSet = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLog = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuEnd = new System.Windows.Forms.ToolStripMenuItem();
            this.nmuBackup = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainIcon
            // 
            this.mainIcon.ContextMenuStrip = this.mainMenu;
            this.mainIcon.Text = "감시";
            this.mainIcon.Visible = true;
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nmuBackup,
            this.nmuInitDataSet,
            this.mnuLog,
            this.toolStripMenuItem1,
            this.mnuEnd});
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(181, 120);
            // 
            // nmuInitDataSet
            // 
            this.nmuInitDataSet.Name = "nmuInitDataSet";
            this.nmuInitDataSet.Size = new System.Drawing.Size(180, 22);
            this.nmuInitDataSet.Text = "초기정보등록";
            this.nmuInitDataSet.Click += new System.EventHandler(this.nmuInitDataSet_Click);
            // 
            // mnuLog
            // 
            this.mnuLog.Name = "mnuLog";
            this.mnuLog.Size = new System.Drawing.Size(180, 22);
            this.mnuLog.Text = "로그보기";
            this.mnuLog.Click += new System.EventHandler(this.mnuLog_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
            // 
            // mnuEnd
            // 
            this.mnuEnd.Name = "mnuEnd";
            this.mnuEnd.Size = new System.Drawing.Size(180, 22);
            this.mnuEnd.Text = "종료";
            this.mnuEnd.Click += new System.EventHandler(this.mnuEnd_Click);
            // 
            // nmuBackup
            // 
            this.nmuBackup.Name = "nmuBackup";
            this.nmuBackup.Size = new System.Drawing.Size(180, 22);
            this.nmuBackup.Text = "백업";
            this.nmuBackup.Click += new System.EventHandler(this.nmuBackup_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 104);
            this.Name = "frmMain";
            this.ShowInTaskbar = false;
            this.Text = "main";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.mainMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon mainIcon;
        private System.Windows.Forms.ContextMenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuLog;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuEnd;
        private System.Windows.Forms.ToolStripMenuItem nmuInitDataSet;
        private System.Windows.Forms.ToolStripMenuItem nmuBackup;
    }
}

