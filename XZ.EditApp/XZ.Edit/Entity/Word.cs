using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Entity {
    public class Word {

        public Word() { }
        public Word(string text) {
            this.Text = text;
        }

        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public int Length {
            get {
                if (this.PEWordType == EWordType.Tab)
                    return 1;
                return this.Text.Length;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public EWordType PEWordType { get; set; }

        /// <summary>
        /// 样式
        /// </summary>
        public WFontColor PFont { get; set; }

        /// <summary>
        /// 包含的样式
        /// </summary>
        public WFontColor PIncluedFont { get; set; }

        /// <summary>
        /// 在行中的位置
        /// </summary>
        public int LineIndex { get; set; }
    }
}
