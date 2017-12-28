using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XZ.EditApp {
    public partial class FindText : Form {
        public FindText() {
            InitializeComponent();
        }

        public Action<XZ.Edit.Entity.FindText> CallBack { get; set; }

        private void but_find_Click(object sender, EventArgs e) {
            var fd = new XZ.Edit.Entity.FindText() {
                FindString = this.tbox_findText.Text,
                IgnoreCase = !this.check_IgnoreCase.Checked,
                IsRegex = this.check_isRegex.Checked,
                Multiline = this.check_Multiline.Checked
            };
            if (this.CallBack != null)
                CallBack(fd);
        }
    }
}
