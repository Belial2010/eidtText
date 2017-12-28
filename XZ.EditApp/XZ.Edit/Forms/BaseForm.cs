using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XZ.Edit.Forms {
    public class BaseForm : Form {
        private int _itemHeight;

        /// <summary>
        /// 获取项的高度
        /// </summary>
        protected int GetItemHeight {
            get {
                if (this._itemHeight == 0)
                    this._itemHeight = FontContainer.GetFontHeight(this.GetFont);

                return _itemHeight;
            }
        }

        protected Font GetFont {
            get {
                return new Font(FontContainer.DefaultFont.Name, this.Font.Size, this.Font.Style);
            }
        }
    }
}
