namespace XZ.EditApp {
    partial class FindText {
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
            this.check_IgnoreCase = new System.Windows.Forms.CheckBox();
            this.check_Multiline = new System.Windows.Forms.CheckBox();
            this.check_isRegex = new System.Windows.Forms.CheckBox();
            this.tbox_findText = new System.Windows.Forms.TextBox();
            this.but_find = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // check_IgnoreCase
            // 
            this.check_IgnoreCase.AutoSize = true;
            this.check_IgnoreCase.Location = new System.Drawing.Point(31, 88);
            this.check_IgnoreCase.Name = "check_IgnoreCase";
            this.check_IgnoreCase.Size = new System.Drawing.Size(101, 19);
            this.check_IgnoreCase.TabIndex = 0;
            this.check_IgnoreCase.Text = "区分大小写";
            this.check_IgnoreCase.UseVisualStyleBackColor = true;
            // 
            // check_Multiline
            // 
            this.check_Multiline.AutoSize = true;
            this.check_Multiline.Location = new System.Drawing.Point(156, 88);
            this.check_Multiline.Name = "check_Multiline";
            this.check_Multiline.Size = new System.Drawing.Size(56, 19);
            this.check_Multiline.TabIndex = 1;
            this.check_Multiline.Text = "多行";
            this.check_Multiline.UseVisualStyleBackColor = true;
            // 
            // check_isRegex
            // 
            this.check_isRegex.AutoSize = true;
            this.check_isRegex.Location = new System.Drawing.Point(234, 88);
            this.check_isRegex.Name = "check_isRegex";
            this.check_isRegex.Size = new System.Drawing.Size(131, 19);
            this.check_isRegex.TabIndex = 2;
            this.check_isRegex.Text = "使用正则表达式";
            this.check_isRegex.UseVisualStyleBackColor = true;
            // 
            // tbox_findText
            // 
            this.tbox_findText.Location = new System.Drawing.Point(31, 13);
            this.tbox_findText.Multiline = true;
            this.tbox_findText.Name = "tbox_findText";
            this.tbox_findText.Size = new System.Drawing.Size(334, 60);
            this.tbox_findText.TabIndex = 3;
            // 
            // but_find
            // 
            this.but_find.Location = new System.Drawing.Point(270, 129);
            this.but_find.Name = "but_find";
            this.but_find.Size = new System.Drawing.Size(75, 23);
            this.but_find.TabIndex = 4;
            this.but_find.Text = "查找";
            this.but_find.UseVisualStyleBackColor = true;
            this.but_find.Click += new System.EventHandler(this.but_find_Click);
            // 
            // FindText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 171);
            this.Controls.Add(this.but_find);
            this.Controls.Add(this.tbox_findText);
            this.Controls.Add(this.check_isRegex);
            this.Controls.Add(this.check_Multiline);
            this.Controls.Add(this.check_IgnoreCase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FindText";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "查找文本";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox check_IgnoreCase;
        private System.Windows.Forms.CheckBox check_Multiline;
        private System.Windows.Forms.CheckBox check_isRegex;
        private System.Windows.Forms.TextBox tbox_findText;
        private System.Windows.Forms.Button but_find;
    }
}