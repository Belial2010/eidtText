using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XZ.Edit.Entity;
using XZ.Edit.Interfaces;

namespace XZ.Edit.Forms {

    /// <summary>
    /// 补全窗口选择事件
    /// </summary>
    /// <param name="keyData"></param>
    /// <returns></returns>
    public delegate bool CompletionWindowSelectEventHandler(Keys keyData);

    /// <summary>
    /// 补全窗口
    /// </summary>
    public class CompletionWindow : BaseForm {
        private Form _parentForm;
        private IEdit _iedit;
        private CompletionData[] _datas;
        private const int space = 10;
        private const int rightSpace = 50;
        private Point curosrXYIndex;
        private bool _isMouseWheel;
        private int _selectIndex = -1;
        private int _maxWidth;
        private CToolTip _toolTip;
        private Rectangle _screeRect;
        private Dictionary<string, int> dictIndex = new Dictionary<string, int>();
        private string _writeText;
        private int locationY;
        private int locationX;
        private int lineStringIndexX;
        public CompletionWindow(IEdit control, CompletionData[] cdata) {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            this._parentForm = control.GetParentForm;
            this._iedit = control;
            StartPosition = FormStartPosition.Manual;
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            this._datas = cdata;
            Array.Sort(cdata);
            Size = new Size(0, 0);
            this.locationY = this._iedit.GetCursor.CousorPointForEdit.Y - this._iedit.GetVerticalScrollValue + FontContainer.FontHeight + 2;
            this._iedit.AddCompletionWindow(this);
            this.curosrXYIndex = new Point(control.GetCursor.CousorPointForWord.X, control.GetCursor.CousorPointForWord.Y);
            control.CompletionWindowSelectEvent += control_CompletionWindowSelect;
            //control.LostFocus += control_LostFocus;
            this._parentForm.LocationChanged += WindowLocationChanged;
            this._screeRect = Screen.GetWorkingArea(this);

        }

        /// <summary>
        ///  设置开始位置
        /// </summary>
        /// <param name="x"></param>
        public void SetlocationX(int x, int lineIndexX) {
            this.locationX = x;
            this.lineStringIndexX = lineIndexX;
        }

        //void control_LostFocus(object sender, EventArgs e) {
        //    if (!_iedit.Focused && !this.ContainsFocus)
        //        this.Close();
        //}

        void WindowLocationChanged(object sender, EventArgs e) {
            var location = this._iedit.GetPointCursorForScreen(new Point(this.locationX, this.locationY));
            if (this.Height + location.Y > this._screeRect.Height)
                location.Y = location.Y - this.Height - FontContainer.FontHeight;
            this.Location = location;
            //this.Opacity = 1;
            this.SetTipLocation();
            
        }

        bool control_CompletionWindowSelect(Keys keyData) {
            switch (keyData) {
                case Keys.Up:
                    if (this._selectIndex > 0) {
                        this._selectIndex--;
                        this.SetKeyToShowDescScrollValue();
                        this.Invalidate();
                    }
                    return true;
                case Keys.Down:
                    if (this._selectIndex < this._datas.Length - 1) {
                        this._selectIndex++;
                        this.SetKeyToShowDescScrollValue();
                        this.Invalidate();
                    }
                    return true;
                case Keys.Enter:
                    if (this._selectIndex <= -1)
                        return false;
                    this.InsertString();
                    return true;
                case Keys.Tab:
                    if (this._selectIndex <= -1)
                        this._selectIndex = 0;
                    this.InsertString();
                    return true;
                case Keys.Escape:
                    this.Close();
                    return true;
            }
            return false;
        }

        private void SetKeyToShowDescScrollValue() {
            var data = this._datas[this._selectIndex];
            this.ShowDescription(data.Description);
            int i = this.VerticalScroll.Value / this.GetItemHeight;
            int end = Math.Min(10 + i, this._datas.Length);
            int value = -1;
            if (i > _selectIndex)
                value = this._selectIndex * this.GetItemHeight;
            else if (end <= _selectIndex)
                value = (i + 1) * this.GetItemHeight;

            if (value > -1) {
                this.VerticalScroll.Value = value;
                this.UpdateScrollbars();
            }
        }

        private void InsertString() {
            var data = this._datas[this._selectIndex].Text;

            var chars = this._iedit.GetParser.PLanguageMode.ConcatChars;
            if (chars != null && chars.Length > 0) {
                foreach (var c in chars) {
                    if (this._writeText != null && this._writeText.StartsWith(c)) {
                        data = c + data;
                        break;
                    }
                }
            }
            int y = this._iedit.GetCursor.CousorPointForEdit.Y;
            var ls = this._iedit.GetParser.PLineString[this._iedit.GetCursor.CousorPointForWord.Y];
            var startPoint = new CPoint(this.locationX, y, ls.Width, this.lineStringIndexX - 1);
            var endPoint = new CPoint(this._iedit.GetCursor.CousorPointForEdit.X, y, ls.Width, this._iedit.GetCursor.CousorPointForWord.X);

            this._iedit.GetParser.SetBgStartPoint(startPoint);
            this._iedit.GetParser.SetBgEndPoint(endPoint);

            var action = new Actions.PasteAction(this._iedit.GetParser);
            action.PPasteText = data;
            action.Execute();

            this._iedit.GetParser.AddAction(action);
            this.Close();
        }

        /// <summary>
        /// 更改了字符
        /// </summary>
        public void ChangeChar() {
            int y = this.curosrXYIndex.Y;
            string value = this._iedit.GetParser.PLineString[y].Text;
            int length = this._iedit.GetCursor.CousorPointForWord.X - this.curosrXYIndex.X + 1;
            //int length = 1;
            if (length < 0 || y != this.curosrXYIndex.Y) {
                this.Close();
                return;
            }
            if (this.curosrXYIndex.X > -1) {
                value = value.Substring(this.curosrXYIndex.X, length).Trim();
                if (string.IsNullOrEmpty(value)) {
                    this.Close();
                    return;
                }
                this._writeText = value;
                if (this._iedit.GetParser.PLanguageMode.ConcatChars != null && value != null) {
                    foreach (var cc in this._iedit.GetParser.PLanguageMode.ConcatChars)
                        if (value.StartsWith(cc)) {
                            value = value.Substring(cc.Length);
                            break;
                        }
                }

                this._selectIndex = -1;
                int maxwidth = 0;
                int maxIndex = 0;
                int i = 0;
                if (this.dictIndex.ContainsKey(value)) {
                    this._selectIndex = this.dictIndex[value];
                    this.Invalidate();
                    this.SetVerticalScroll();
                    this.ShowDescription(this._datas[this._selectIndex].Description);
                    return;
                }
                if (value.Length > 1) {
                    string key = value.Substring(0, value.Length - 1);
                    if (this.dictIndex.ContainsKey(key))
                        i = this.dictIndex[key];
                }
                for (; i < this._datas.Length; i++) {
                    var d = this._datas[i];
                    if (this._selectIndex < 0 && d.Text.StartsWith(value, StringComparison.OrdinalIgnoreCase)) {
                        this._selectIndex = i;
                        dictIndex[value] = i;

                        if (this.VerticalScroll.Visible && this.VerticalScroll.Value >= (i + 1) * this.GetItemHeight) {
                            this.VerticalScroll.Value = i * this.GetItemHeight;
                            UpdateScrollbars();
                        }

                        if (this._maxWidth > 0)
                            break;
                    }
                    if (d.Text.Length > maxwidth) {
                        maxwidth = d.Text.Length;
                        maxIndex = i;
                    }
                }
                #region 设置高度和宽度
                if (this._maxWidth == 0) {
                    this._maxWidth = maxwidth;
                    string text = this._datas[maxIndex].Text;
                    int width = CharCommand.GetCharWidth(this.CreateGraphics(), text, this.GetFont);
                    int height = Math.Min(10, this._datas.Length) * this.GetItemHeight + space * 2;
                    this.Width = Math.Max(300, width + rightSpace);
                    this.Height = height;
                    SetScroll(this.Width, this.Height);
                    this.WindowLocationChanged(null, null);
                }
                this.SetVerticalScroll();
                #endregion
                this.Invalidate();
                if (this._selectIndex > -1)
                    this.ShowDescription(this._datas[this._selectIndex].Description);
                else if (this._toolTip != null && !this._toolTip.IsHide)
                    this._toolTip.Hide();

            }
        }

        private void SetVerticalScroll() {
            if (_selectIndex > -1 && _selectIndex * this.GetItemHeight + space * 2 >= this.Height) {
                VerticalScroll.Value = Math.Min(_selectIndex * this.GetItemHeight, this.GetMaxHeight() - this.Height);
                this.UpdateScrollbars();
            } else {
                VerticalScroll.Value = 0;
                this.UpdateScrollbars();
            }
        }

        /// <summary>
        /// 显示描述
        /// </summary>
        /// <param name="desc"></param>
        private void ShowDescription(string desc) {
            if (this._toolTip == null) {
                this._toolTip = new CToolTip(this._parentForm, this._iedit.GetParser.PLanguageMode.TabSpaceCount);
                this._toolTip.IsChangeLeftRight = false;
            }
            if (string.IsNullOrWhiteSpace(desc)) {
                this._toolTip.Hide();
                return;
            }
            this._toolTip.Opacity = 0;
            this._toolTip.SetText(desc);

            if (this._toolTip.IsHide)
                this._toolTip.Show();

            this.SetTipLocation();
            this._toolTip.Refresh();

        }

        private void SetTipLocation() {
            if (this._toolTip != null) {
                var w = this._toolTip.Width;
                int x = this.Location.X;
                int y = this._screeRect.Width - x - this.Width;
                int left = 0;
                if (x > y)
                    left = x - w;
                else
                    left = x + this.Width;

                //this._toolTip.Location = new Point(left, this.Location.Y);
                this._toolTip.SetPosition(new Point(left, this.Location.Y), this.Location.Y);
                this._toolTip.Opacity = 1;
            }
        }

        private void SetScroll(int width, int height) {
            int maxHeight = this.GetMaxHeight();
            if (this.GetItemHeight > 0 && height < maxHeight) {
                //this.Width += this.sc
                this.AutoScrollMinSize = new Size(width - 30, maxHeight);
            }
        }

        private int GetMaxHeight() {
            return this.GetItemHeight * this._datas.Length + space * 2;
        }

        private static SolidBrush SelectLineBackGoundBrush = new SolidBrush(Color.FromArgb(51, 153, 255));
        private static Pen DefaultSelectLinePen = new Pen(new SolidBrush(Color.FromArgb(51, 153, 255)), 1);
        /// <summary>
        /// 显示下来框
        /// </summary>
        public void ShowCompletionWindow() {
            if (this._datas == null || this._datas.Length <= 0)
                return;
            Owner = _parentForm;
            Enabled = true;

            VerticalScroll.SmallChange = this.GetItemHeight;
            VerticalScroll.LargeChange = 10 * this.GetItemHeight;
            this.Show();
        }

        public void SetMouseWheel(MouseEventArgs e) {
            int linesPerClick = Math.Max(SystemInformation.MouseWheelScrollLines, 1);
            if (VerticalScroll.Visible) {
                _isMouseWheel = true;
                int value = linesPerClick * Math.Sign(e.Delta) * this.GetItemHeight;
                value = VerticalScroll.Value - value;
                var ea = new ScrollEventArgs(e.Delta > 0 ? ScrollEventType.SmallDecrement : ScrollEventType.SmallIncrement,
                                        VerticalScroll.Value,
                                        value,
                                        ScrollOrientation.VerticalScroll);

                OnScroll(ea);
                ((HandledMouseEventArgs)e).Handled = true;
                this.Invalidate();
            }
        }

        private void UpdateScrollbars() {
            this.AutoScrollMinSize -= new Size(1, 0);
            this.AutoScrollMinSize += new Size(1, 0);
        }

        private int MouseSelectIndex(MouseEventArgs e) {
            int y = e.Y - space + this.VerticalScroll.Value;
            int index = y / this.GetItemHeight;
            if (index < this._datas.Length) {
                this._selectIndex = index;
                return index;
            }
            return -1;
        }

        protected override void OnPaint(PaintEventArgs e) {
            var g = e.Graphics;
            int i = this.VerticalScroll.Value / this.GetItemHeight;
            int end = Math.Min(10 + i, this._datas.Length);
            int top = 0;
            int length = end - i;
            var f = this.GetFont;
            int maxWidth = 0;
            Color foreColor = Color.Empty;
            //ifs

            for (; i < end; i++) {
                var d = this._datas[i];
                foreColor = this.ForeColor;
                top = i * this.GetItemHeight + space - this.VerticalScroll.Value;
                #region 绘制默认背景
                if (this._selectIndex == -1 && i == 0)
                    g.DrawRectangle(DefaultSelectLinePen, new Rectangle(20, top, this.Width, this.GetItemHeight));

                #endregion
                #region 绘制选择背景
                if (i == this._selectIndex) {
                    g.FillRectangle(SelectLineBackGoundBrush, new Rectangle(20, top, this.Width, this.GetItemHeight));
                    foreColor = Color.White;
                }
                #endregion
                #region 绘制ICO图标
                g.DrawImage(ResList.ImageList[d.ImageIndex], new Rectangle(2, top, 16, 16));
                #endregion
                #region 绘制文本
                TextRenderer.DrawText(g, d.Text,
                    f,
                    new Point(22, top),
                    foreColor,
                    CharCommand.CTextFormatFlags);
                int width = CharCommand.GetCharWidth(g, d.Text, f);
                maxWidth = Math.Max(maxWidth, width);
                #endregion
            }
            #region 绘制边框
            g.DrawRectangle(new Pen(new SolidBrush(Color.FromArgb(239, 239, 242))),
                    new Rectangle(0, 0, this.Width - 1, this.Height - 1));
            #endregion
        }

        protected override void OnPaintBackground(PaintEventArgs pe) {
            pe.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(246, 246, 246)), pe.ClipRectangle);
        }

        protected override void OnClosed(EventArgs e) {
            _iedit.CompletionWindowSelectEvent -= control_CompletionWindowSelect;
            if (this._toolTip != null)
                this._toolTip.Close();
            base.OnClosed(e);
        }

        protected override bool ShowWithoutActivation {
            get {
                return true;
            }
        }

        protected override void OnScroll(ScrollEventArgs se) {
            int value = Math.Max(VerticalScroll.Minimum, (se.NewValue / this.GetItemHeight) * this.GetItemHeight);
            this.VerticalScroll.Value = value;
            if (_isMouseWheel) {
                UpdateScrollbars();
                _isMouseWheel = false;
            }
            base.OnScroll(se);
        }

        protected override void OnMouseWheel(MouseEventArgs e) {
            this.SetMouseWheel(e);
        }

        protected override void WndProc(ref Message m) {
            if (m.Msg == CharCommand.WM_HSCROLL || m.Msg == CharCommand.WM_VSCROLL)
                if (m.WParam.ToInt32() != CharCommand.SB_ENDSCROLL)
                    Invalidate();

            base.WndProc(ref m);

        }

        protected override void OnMouseDown(MouseEventArgs e) {
            //base.OnMouseDown(e);
            int index = this.MouseSelectIndex(e);
            if (index > -1) {
                string desc = this._datas[index].Description;
                this.Invalidate();
                this.ShowDescription(desc);
                this._iedit.GetFocues();
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e) {
            base.OnMouseDoubleClick(e);
            int index = this.MouseSelectIndex(e);
            if (index > -1) {
                InsertString();
                this._iedit.GetFocues();
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e) {
            base.OnKeyPress(e);
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);
            control_CompletionWindowSelect(e.KeyData);
        }

    }
}
