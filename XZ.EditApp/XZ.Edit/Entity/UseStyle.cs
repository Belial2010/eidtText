using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Entity {
    public class UseStyle {

        public EUseStyle PEUserStyle { get; set; }
        public WFontColor PWFontColor { get; set; }

    }

    public enum EUseStyle {
        Before,
        After
    }
}
