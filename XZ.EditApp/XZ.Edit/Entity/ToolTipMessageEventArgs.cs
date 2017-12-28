using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using XZ.Edit.Interfaces;

namespace XZ.Edit.Entity {

    public delegate void ToolTipMessageEventHandler(IEdit sender, ToolTipMessageEventArgs e);

    public class ToolTipMessageEventArgs {

        private string _msg;
        /// <summary>
        /// 坐标
        /// </summary>
        //public Point Point { get; set; }

        /// <summary>
        /// 行文本
        /// </summary>
        public LineString LineString { get; set; }

        /// <summary>
        /// 选择的单个文本
        /// </summary>
        public char PChar { get; set; }

        /// <summary>
        /// 选择的文本
        /// </summary>
        public string Word { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int WordStartIndex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int WordIndex { get; set; }

        /// <summary>
        /// 行所在的索引
        /// </summary>
        public int TextIndex { get; set; }

        /// <summary>
        /// 整个列表索引
        /// </summary>
        public int LineStringIndex { get; set; }

        /// <summary>
        /// 文本宽度
        /// </summary>
        internal int WordWidth { get; set; }

        /// <summary>
        /// 显示提示
        /// </summary>
        /// <param name="msg"></param>
        public void ShowToolTip(string msg) {
            this._msg = msg;
        }

        /// <summary>
        /// 获取提示信息
        /// </summary>
        /// <returns></returns>
        internal string GetToolTipMessage() {
            return this._msg;
        }
    }
}
