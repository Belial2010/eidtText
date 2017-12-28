using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Entity {
    public class FindTextLocation {
        public int ID { get; set; }
        public string FineString { get; set; }
        public int IndexX { get; set; }
        public int X { get; set; }
        public int Width { get; set; }
        public int NextFindIndex { get; set; }

        public List<FindTextLocation> NextFindTextLocations { get; set; }
    }
}
