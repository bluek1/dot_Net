namespace DownWatch {
    partial class frmNanBack {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.progBackUp = new System.Windows.Forms.ProgressBar();
            this.progFileCopy = new System.Windows.Forms.ProgressBar();
            this.labMsg = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progBackUp
            // 
            this.progBackUp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progBackUp.Location = new System.Drawing.Point(12, 75);
            this.progBackUp.Name = "progBackUp";
            this.progBackUp.Size = new System.Drawing.Size(412, 16);
            this.progBackUp.TabIndex = 0;
            // 
            // progFileCopy
            // 
            this.progFileCopy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progFileCopy.Location = new System.Drawing.Point(12, 128);
            this.progFileCopy.Name = "progFileCopy";
            this.progFileCopy.Size = new System.Drawing.Size(412, 16);
            this.progFileCopy.TabIndex = 1;
            // 
            // labMsg
            // 
            this.labMsg.AutoSize = true;
            this.labMsg.Location = new System.Drawing.Point(12, 9);
            this.labMsg.Name = "labMsg";
            this.labMsg.Size = new System.Drawing.Size(69, 12);
            this.labMsg.TabIndex = 2;
            this.labMsg.Text = "파일 백업중";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(412, 21);
            this.label1.TabIndex = 3;
            this.label1.Text = "백업진행도";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(412, 21);
            this.label2.TabIndex = 4;
            this.label2.Text = "복사진행도";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.UseCompatibleTextRendering = true;
            // 
            // frmNanBack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 159);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labMsg);
            this.Controls.Add(this.progFileCopy);
            this.Controls.Add(this.progBackUp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmNanBack";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "파일 백업";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmNanBack_FormClosed);
            this.Load += new System.EventHandler(this.frmNanBack_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progBackUp;
        private System.Windows.Forms.ProgressBar progFileCopy;
        private System.Windows.Forms.Label labMsg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}