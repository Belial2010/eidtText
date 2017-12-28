using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace XZ.Edit.Entity {
    public class LanguageMode {
   

        /// <summary>
        /// 制表符多少个空格
        /// </summary>
        public int TabSpaceCount { get; set; }

        /// <summary>
        /// 分割字符
        /// </summary>
        public HashSet<Char> CompartChars { get; set; }

        /// <summary>
        /// 回车缩进开头字符串
        /// </summary>
        public HashSet<string> Retraction { get; set; }

        /// <summary>
        /// 回车缩进光标所在前面字符
        /// </summary>
        public HashSet<char> RetractionBerforChars { get; set; }

        /// <summary>
        /// 回车缩进光标所在后面不能出现的字符
        /// </summary>
        public HashSet<char> RetractionAfterNoChar { get; set; }

        /// <summary>
        /// 回车之前如果出现这些字符，则不缩进
        /// </summary>
        //public HashSet<char> RetractionNoChar { get; set; }

        /// <summary>
        /// 回车之前，光标后面出现这些字符。则不缩进
        /// </summary>
        //public HashSet<char> RetractionAfterFirsNoChar { get; set; }

        /// <summary>
        /// 分割字符颜色
        /// </summary>
        public Dictionary<char, WFontColor> CompartCharFont { get; set; }

        /// <summary>
        /// 成对字符开始
        /// </summary>
        public Dictionary<char, char> CoupleStart { get; set; }

        /// <summary>
        /// 成对字符背景
        /// </summary>
        public SolidBrush CoupleBackGround { get; set; }

        /// <summary>
        /// 成对字符结束
        /// </summary>
        public Dictionary<char, char> CoupleEnd { get; set; }

        /// <summary>
        /// 块
        /// </summary>
        public Tuple<char, char> Range { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public Dictionary<string, WFontColor> WordFonts { get; set; }

        public StartWordStyle PStartWordStyle { get; set; }
        public EndWordStyle PEndWordStyle { get; set; }
        public Dictionary<char, UseStyle> CharUseStyle { get; set; }
        public Dictionary<string, UseStyle> TagUseStyle { get; set; }

        /// <summary>
        /// 自动添加字符
        /// </summary>
        public Dictionary<char, char> AutoInsertChar { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string[] ConcatChars { get; set; }

        public Dictionary<string, UseWordFont> UseBefore { get; set; }

        public Dictionary<string, UseWordFont> UseAfter { get; set; }

    }
}
