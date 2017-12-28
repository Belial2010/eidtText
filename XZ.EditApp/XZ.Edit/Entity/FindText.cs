using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Entity {
    /// <summary>
    /// 操作文本
    /// </summary>
    public class FindText {

        /// <summary>
        /// 要操作的文本
        /// </summary>
        public string FindString { get; set; }

        /// <summary>
        /// 多行
        /// </summary>
        public bool Multiline { get; set; }

        /// <summary>
        /// 区分大小写 true 不区分，false 区分
        /// </summary>
        public bool IgnoreCase { get; set; }

        /// <summary>
        /// 正则表达式
        /// </summary>
        public bool IsRegex { get; set; }
    }
}
