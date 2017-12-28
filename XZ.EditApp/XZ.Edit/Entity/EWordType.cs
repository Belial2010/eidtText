using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Entity {
    [Flags]
    public enum EWordType {
        Word = 1,
        Space = 2,
        Tab = 4,
        Compart = 8,
        //HidePucker = 16,
        _None = 0
    }
}
