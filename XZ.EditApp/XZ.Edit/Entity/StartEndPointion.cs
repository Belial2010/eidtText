using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Entity {
    public class StartEndPointion {
        public StartEndPointion(int start, int end) {
            this.Start = start;
            this.End = end;
            this.EndID = -1;
        }
        public LineNodeProperty LNProperty { get; set; }
        public int EndID { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
    }
}
