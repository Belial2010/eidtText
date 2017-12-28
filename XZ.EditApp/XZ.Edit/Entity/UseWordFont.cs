using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Entity {
    /// <summary>
    /// 自定义文本颜色
    /// </summary>
    public class UseWordFont {

        /// <summary>
        /// 颜色
        /// </summary>
        public WFontColor PFont { get; set; }

        public UseWordFontType PType { get; set; }

        public Dictionary<string, UseWordFont> After { get; set; }
        
        public Dictionary<string, UseWordFont> Before { get; set; }

    }

    public enum UseWordFontType { 
        Before,
        After
    }
   
}
