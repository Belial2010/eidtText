using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Entity {
    public class SursorSelectWord {
        public SursorSelectWord() {
            this.LineIndex = -1;
        }

        /// <summary>
        /// 选择的文本标签
        /// </summary>
        public Word PWord { get; set; }

        /// <summary>
        /// 在文本标签的位置
        /// </summary>
        public int PWordIndex { get; set; }

        /// <summary>
        /// 离左边的宽度，不计算折叠和数行
        /// </summary>
        public int LeftWidth { get; set; }

        public int LeftWidthForWord { get; set; }

        public int LineIndex { get; set; } 

        public bool End { get; set; }

        /// <summary>
        /// 文本标签离左边的距离 左边默认不算
        /// </summary>
        public int WordLeftWidth { get; set; }

    }
}
