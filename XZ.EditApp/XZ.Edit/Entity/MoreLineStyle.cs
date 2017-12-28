using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Entity {
    /// <summary>
    /// 多行样式
    /// </summary>
    public class MoreLineStyle {

        public MoreLineStyle() {
            //EndIndexY = int.MaxValue;
        }

        /// <summary>
        /// 开始标签
        /// </summary>
        public bool IsStart { get; set; }

        public string Tag { get; set; }

        /// <summary>
        /// 所在的位置
        /// </summary>
        public int WordsIndex { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public WFontColor PFontColor { get; set; }

        public int IndexY { get; set; }

        //public int EndIndexY { get; set; }
    }
}
