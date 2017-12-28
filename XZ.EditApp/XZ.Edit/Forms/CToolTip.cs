using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XZ.Edit.Entity;

namespace XZ.Edit.Forms {
    public class CToolTip : BaseForm {
        private string _text;
        private int _tabIndent;

        public CToolTip(Form parent, int tabIndent) {
            SetStyle(ControlStyles.Selectable, false);
            StartPosition = FormStartPosition.Manual;
            FormBorderStyle = FormBorderStyle.None;
            Owner = parent;
            ShowInTaskbar = false;
            Size = new Size(0, 0);
            base.CreateHandle();
            this._tabIndent = tabIndent;
            this.IsHide = true;
            this.maxWidth = SystemInformation.WorkingArea.Width;
            this.IsChangeLeftRight = true;
        }

        private int downMaxHeight;
        private int upMaxHeight;
        private int maxWidth;

        /// <summary>
        /// 是否允许左右移动
        /// </summary>
        public bool IsChangeLeftRight { get; set; }
        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool IsHide {
            get;
            set;
        }

        /// <summary>
        /// 设置文本值
        /// </summary>
        public void SetText(string msg) {
            this._text = msg;
            this.pLineStrings = null;
            //this.Invalidate();
        }

        private LineString[] pLineStrings;

        public void SetLineString(LineString[] ls) {
            this.pLineStrings = ls;
            this._text = null;
            //this.Invalidate();
        }

        /// <summary>
        /// 设置坐标
        /// </summary>
        /// <param name="position"></param>
        /// <param name="y"></param>
        public void SetPosition(Point position, int y) {
            this.Location = position;
            this.downMaxHeight = SystemInformation.WorkingArea.Height - position.Y;
            this.upMaxHeight = Math.Min(Control.MousePosition.Y, y);
        }

        protected override void OnPaint(PaintEventArgs e) {
            if (this.pLineStrings == null)
                this.PointText(e.Graphics);
            else
                this.PointLs(e.Graphics);
        }

        private void PointText(Graphics g) {
            string[] array = this._text.Split(CharCommand.Char_Newline);
            int y = 10;
            int maxWidth = 0;
            var tuple = this.GetContentSize(array.Length);
            int count = Math.Min(tuple.Item2, array.Length);
            for (var i = 0; i < count; i++) {
                var cs = array[i];
                var line = cs.Trim(CharCommand.Char_Newline);
                
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                int width = 10;
                var words = CharCommand.CompartString(line, CharCommand.Char_Tab, CharCommand.Char_Space);
                foreach (var w in words) {
                    if (string.IsNullOrEmpty(w.Text))
                        continue;
                    switch (w.PEWordType) {
                        case Entity.EWordType.Word:
                            TextRenderer.DrawText(g, w.Text, this.GetFont, new Point(width, y), FontContainer.ForeColor, CharCommand.CTextFormatFlags);
                            width += CharCommand.GetCharWidth(g, w.Text, this.GetFont);
                            break;
                        case Entity.EWordType.Tab:
                            width += FontContainer.GetSpaceWidth(g) * _tabIndent;
                            break;
                        default:
                            width += FontContainer.GetSpaceWidth(g);
                            break;
                    }
                }
                y += this.GetItemHeight;
                maxWidth = Math.Max(width, maxWidth);
            }
            if (tuple.Item2 < array.Length) {
                TextRenderer.DrawText(g, "···", this.GetFont, new Point(10, y), FontContainer.ForeColor, CharCommand.CTextFormatFlags);
                y += this.GetItemHeight;
            }
            this.Width = maxWidth + 10;
            this.Height = y + 10;
            this.ResetLocation(tuple.Item1);
            g.DrawRectangle(BroderPen, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        private void PointLs(Graphics g) {
            int maxWidth = 0;
            int y = 10;
            var tuple = this.GetContentSize(this.pLineStrings.Count());
            int count = Math.Min(tuple.Item2, this.pLineStrings.Count());
            for (var i = 0; i < count; i++) {
                var cs = this.pLineStrings[i];
                int width = 10;
                foreach (var w in cs.PWord) {
                    switch (w.PEWordType) {
                        case EWordType.Word:
                        case EWordType.Compart:
                            TextRenderer.DrawText(g, w.Text, this.GetFont, new Point(width, y), FontContainer.ForeColor, CharCommand.CTextFormatFlags);
                            width += CharCommand.GetCharWidth(g, w.Text, this.GetFont);
                            break;
                        default:
                            if (w.Width == 0 && !string.IsNullOrEmpty(w.Text)) {
                                w.Width = FontContainer.GetSpaceWidth(g);
                                if (w.PEWordType == EWordType.Tab)
                                    w.Width *= _tabIndent;
                            }
                            width += w.Width;
                            break;
                    }
                }
                maxWidth = Math.Max(width, maxWidth);
                y += this.GetItemHeight;
            }
            if (tuple.Item2 < this.pLineStrings.Count()) {
                TextRenderer.DrawText(g, "···", this.GetFont, new Point(10, y), FontContainer.ForeColor, CharCommand.CTextFormatFlags);
                y += this.GetItemHeight;
            }
            this.Width = maxWidth + 10;
            this.Height = y + 10;
            this.ResetLocation(tuple.Item1);
            g.DrawRectangle(BroderPen, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
        }

        /// <summary>
        ///  重置坐标
        /// </summary>
        /// <param name="p"></param>
        private void ResetLocation(int p) {
            if (this.IsChangeLeftRight && Location.X + this.Width > SystemInformation.WorkingArea.Width) {
                int y = Location.Y;
                this.Location = new Point(Math.Max(0, SystemInformation.WorkingArea.Width - this.Width), y);
            }
            if (p == 1) {
                int x = this.Location.X;
                this.Location = new Point(x, this.upMaxHeight - this.Height);
            }
        }

        private Tuple<int, int> GetContentSize(int count) {
            int height = count * this.GetItemHeight + 20;
            if (height >= this.downMaxHeight) {
                if (this.upMaxHeight > this.downMaxHeight + 200) {
                    if (height >= this.upMaxHeight)
                        count = (this.upMaxHeight - 20) / this.GetItemHeight - 1;

                    return Tuple.Create(1, count);
                }
                count = (this.downMaxHeight - 20) / this.GetItemHeight - 1;
            }
            return Tuple.Create(0, count);
        }


        private SolidBrush BackGroundBrush = new SolidBrush(Color.FromArgb(231, 232, 236));
        private Pen BroderPen = new Pen(new SolidBrush(Color.FromArgb(204, 206, 219)), 1);
        /// <summary>
        /// 绘制背景
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaintBackground(PaintEventArgs pe) {
            pe.Graphics.FillRectangle(BackGroundBrush, pe.ClipRectangle);
        }

        protected override bool ShowWithoutActivation {
            get {
                return true;
            }
        }

        public new void Show() {
            this.IsHide = false;
            this.Invalidate();
            base.Show();
        }

        public new void Hide() {
            this.IsHide = true;
            base.Hide();
        }

        //public void 


    }
}
