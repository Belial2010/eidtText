using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Entity {
    /// <summary>
    /// 行字符串
    /// </summary>
    public class LineString {

        public LineString(int id) {
            this.ID = id;
        }

        public int ID { get; set; }

        /// <summary>
        /// 分隔列表
        /// </summary>
        public List<Word> PWord { get; set; }

        private string pText;

        /// <summary>
        /// 文本
        /// </summary>
        public string Text {
            get { return this.pText; }
            set {
                if (this.pText != value)
                    Width = 0;

                this.pText = value;
            }
        }

        /// <summary>
        /// 设置值，该属性不会改变Width
        /// </summary>
        /// <param name="value"></param>
        public void SetText(string value) {
            this.pText = value;
        }
        

        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 字符长度
        /// </summary>
        public int Length { get { return Text.Length; } }

        public LineNodeProperty PLNProperty { get; set; }

        /// <summary>
        /// 跨行样式
        /// </summary>
        public List<MoreLineStyle> PMoreLineStyles { get; set; }


        /// <summary>
        /// 当前行，只在绘制时候赋值
        /// </summary>
        public int IndexY { get; set; }

        /// <summary>
        /// 注释折叠部分
        /// </summary>
        public bool IsCommentPucker { get; set; }
       
    }
}
