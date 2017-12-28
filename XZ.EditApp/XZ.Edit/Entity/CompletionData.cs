using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Entity {
    public class CompletionData : IComparable {

        public CompletionData() { }

        public CompletionData(string text, string desc, int index = 0) {
            this.Text = text;
            this.Description = desc;
            this.ImageIndex = index;
        }

        /// <summary>
        /// 图标
        /// </summary>
        public int ImageIndex {
            get;
            set;
        }

        /// <summary>
        /// 文本
        /// </summary>
        public string Text {
            get;
            set;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description {
            get;
            set;
        }

        public int CompareTo(object obj) {
            if (obj == null || !(obj is CompletionData)) {
                return -1;
            }
            return Text.CompareTo(((CompletionData)obj).Text);
        }
    }
}
