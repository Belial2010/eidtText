using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace XZ.Edit.Entity {
    public class WFontColor {
        /// <summary>
        /// 创建字体样式
        /// </summary>
        /// <param name="f"></param>
        /// <param name="color"></param>
        public WFontColor(Font f, Color color) {
            this.PFont = f;
            this.PColor = color;
        }

        /// <summary>
        /// 创建字体样式
        /// </summary>
        /// <param name="familyName"></param>
        /// <param name="size"></param>
        /// <param name="style"></param>
        /// <param name="color"></param>
        public WFontColor(string familyName, float size, FontStyle style, Color color) {
            this.PFont = new Font(familyName, size, style);
            this.PColor = color;
        }

        /// <summary>
        /// 字体样式
        /// </summary>
        public Font PFont { get; set; }

        /// <summary>
        /// 字体颜色
        /// </summary>
        public Color PColor { get; set; }
    }
}
