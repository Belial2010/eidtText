using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Actions {
    public class NoneAction : BaseAction {
        public NoneAction(Parser paser)
            : base(paser) {
        }

        public Action<BaseAction> RedoExecute { get; set; }
    }
}
