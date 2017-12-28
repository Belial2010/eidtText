using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Entity {
    public class EndWordStyle {

        private Dictionary<char, EndWordStyle> pEndStyle = new Dictionary<char, EndWordStyle>();

        /// <summary>
        /// 是否结束
        /// </summary>
        public bool IsEnd { get; set; }

        public string StartString { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endStr"></param>
        public void Add(string endStr,string startString) {
            if (endStr == null)
                return;
            if (endStr.Length == 0) {
                this.IsEnd = true;
                this.StartString = startString;
            }
            if (string.IsNullOrEmpty(endStr))
                return;
            var c = endStr[0];
            var l = endStr.Substring(1);
            if (pEndStyle.ContainsKey(c))
                pEndStyle[c].Add(l, startString);
            else {
                var endStyle = new EndWordStyle();
                endStyle.Add(l, startString);
                //endStyle.IsEnd = l.Length == 1;

                pEndStyle.Add(c, endStyle);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public EndWordStyle Get(char c) {
            EndWordStyle outValue;
            if (pEndStyle.TryGetValue(c, out outValue))
                return outValue;
            return null;
        }
    }
}
