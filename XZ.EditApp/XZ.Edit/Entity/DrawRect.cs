using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace XZ.Edit.Entity {
    public class DrawRect {

        //public int LsID { get; set; }

        /// <summary>
        /// 包含右侧位置
        /// </summary>
        public int StartX { get; set; }

        /// <summary>
        /// 包含滚动条数据
        /// </summary>
        public int StartY { get; set; }


        //public int Width { get;set; }

        /// <summary>
        /// 包含右侧位置
        /// </summary>
        public int EndX { get; set; }

        /// <summary>
        /// 包含滚动条数据
        /// </summary>
        public int EndY { get; set; }

        /// <summary>
        /// 画笔
        /// </summary>
        public SolidBrush PSolidBrush { get; set; }
             
    }
}
