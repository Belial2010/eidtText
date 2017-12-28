using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Entity {
    public class DrawMLStyle {

        /// <summary>
        /// 所在的位置
        /// </summary>
        public int WordsIndex { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public WFontColor PFontColor { get; set; }

        public int IndexY { get; set; } 
    }
}
