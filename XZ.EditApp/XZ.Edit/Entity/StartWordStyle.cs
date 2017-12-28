using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Entity {
    public class StartWordStyle {

        private Dictionary<char, StartWordStyle> charWC;
        public StartWordStyle() {
            charWC = new Dictionary<char, StartWordStyle>();
            BeforeNoChar = CharCommand.Char_Empty;
            //PWFontColor = wc;

        }

        /// <summary>
        /// 多行
        /// </summary>
        public bool MoreLine { get; set; }

        public StartWordStyle Father { get; private set; }

        /// <summary>
        /// 样式
        /// </summary>
        public WFontColor PWFontColor { get; set; }

        /// <summary>
        /// 当前字符
        /// </summary>
        public char PSEChar { get; set; }

        /// <summary>
        /// 是否一个完整的开头
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// 是否是整行
        /// </summary>
        public bool Line { get; set; }

        /// <summary>
        /// 前面不出现的字符串
        /// </summary>
        public char BeforeNoChar { get; set; }

        /// <summary>
        /// 结束字符
        /// </summary>
        public string EndString { get; set; }

        /// <summary>
        /// 添加字符
        /// </summary>
        /// <param name="c">当前字符</param>
        /// <param name="upChar">上一个字符</param>
        public StartWordStyle Add(char c, char upChar, WFontColor wfc) {
            if (upChar == '\0') {
                this.PSEChar = c;
                if (charWC.ContainsKey(c)) {
                    charWC[c].PWFontColor = wfc;
                    return charWC[c];
                } else {
                    var seWord = new StartWordStyle() { PSEChar = c, PWFontColor = wfc };
                    charWC.Add(c, seWord);
                    seWord.Father = this;
                    return seWord;
                }
                //return this;
            } else {
                StartWordStyle outValue;
                if (charWC.TryGetValue(upChar, out outValue)) {
                    return outValue.Add(c, '\0', wfc);
                }
            }
            return null;
        }

        public StartWordStyle Get(char c) {
            StartWordStyle outValue;
            if (charWC.TryGetValue(c, out outValue))
                return outValue;

            return null;
        }

    }
}
