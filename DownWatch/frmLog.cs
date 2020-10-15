using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DownWatch {
    public partial class frmLog : Form {

        private delegate void SafeCallDelegate(string text);

        public frmLog() {
            InitializeComponent();
        }

        private void frmLog_Load(object sender, EventArgs e) {
            this.Icon=Icon.FromHandle(Properties.Resources.file.GetHicon());
            foreach (String str in Common.logList) {
                txtLog.AppendText("・" + str + Environment.NewLine);
            };
        }

        public void DrawLog(String text) {
            if (txtLog.InvokeRequired) {
                var d = new SafeCallDelegate(DrawLog);
                txtLog.Invoke(d, new object[] { text });
            } else {
                txtLog.AppendText("・" + text + Environment.NewLine);
                if (txtLog.Lines.Length > 100) {
                    txtLog.Text=txtLog.Text.Substring(txtLog.Text.IndexOf(Environment.NewLine) + 1);
                }

            }

        }

        private void btnClose_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
