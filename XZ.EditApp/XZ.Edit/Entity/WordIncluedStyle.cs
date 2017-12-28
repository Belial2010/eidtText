using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Entity {
    public class WordIncluedStyle {

        private StartWordStyle pStartEndWord;
        public WordIncluedStyle(StartWordStyle wEW)
            : this(wEW, null) {

        }

        public WordIncluedStyle(StartWordStyle wEW, string text) {
            pStartEndWord = wEW;
            this.PFontColor = pStartEndWord.PWFontColor.Create();
            this.EndString = wEW.EndString;
            this.SetEndString(wEW.EndString);
            this.Text = text;
        }

        /// <summary>
        /// 前面不出现的字符串
        /// </summary>
        public char BeforeNoChar { get { return pStartEndWord.BeforeNoChar; } }

        /// <summary>
        /// 是否是多行
        /// </summary>
        public bool IsMoreLine { get { return pStartEndWord.MoreLine; } }

        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 是否是一行
        /// </summary>
        public bool Line { get { return pStartEndWord.Line; } }

        /// <summary>
        /// 是否有结束字符
        /// </summary>
        public bool IsEndStr { get; set; }

        /// <summary>
        /// 样式
        /// </summary>
        public WFontColor PFontColor { get; private set; }

        /// <summary>
        /// 第一个结束字符
        /// </summary>
        public char EndFirst { get; private set; }


        /// <summary>
        /// 结束字符串
        /// </summary>
        public string EndString {
            get;
            private set;
        }

        /// <summary>
        /// 设置结束字符
        /// </summary>
        /// <param name="endStr"></param>
        public void SetEndString(string endStr) {
            if (string.IsNullOrEmpty(endStr))
                EndFirst = CharCommand.Char_Empty;
            else {
                IsEndStr = true;
                EndFirst = endStr[0];
            }
        }
    }
}
