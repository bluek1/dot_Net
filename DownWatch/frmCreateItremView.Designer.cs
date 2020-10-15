namespace DownWatch {
    partial class frmCreateItremView {
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
            this.drFileList = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.drFileList)).BeginInit();
            this.SuspendLayout();
            // 
            // drFileList
            // 
            this.drFileList.AllowUserToAddRows = false;
            this.drFileList.AllowUserToDeleteRows = false;
            this.drFileList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.drFileList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.drFileList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.drFileList.Location = new System.Drawing.Point(12, 572);
            this.drFileList.Name = "drFileList";
            this.drFileList.RowTemplate.Height = 23;
            this.drFileList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.drFileList.Size = new System.Drawing.Size(973, 418);
            this.drFileList.TabIndex = 0;
            this.drFileList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.drFileList_CellClick);
            this.drFileList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.drFileList_CellContentClick);
            this.drFileList.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.drFileList_CellEndEdit);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(291, 144);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(57, 61);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmCreateItremView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 1002);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.drFileList);
            this.Name = "frmCreateItremView";
            this.Text = "frmCreateItremView";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmCreateItremView_FormClosed);
            this.Load += new System.EventHandler(this.frmCreateItremView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.drFileList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView drFileList;
        private System.Windows.Forms.Button button1;
    }
}