using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Entity {
    public class FindTextResult {
        public string FindString { get; set; }
        public int StartIndexY { get; set; }
        public int StartIndexX { get; set; }
        public int EndIndexY { get; set; }
        public int EndIndexX { get; set; }
    }
}
