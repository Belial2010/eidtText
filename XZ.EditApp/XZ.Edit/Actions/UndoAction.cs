using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Actions {
    public class UndoAction : BaseAction {
        public UndoAction(Parser paser)
            : base(paser) {
        }

        public override void Execute() {
            if (this.PParser.PUndo.Count == 0)
                return;

            this.PParser.ClearSelect();

            var action = this.PParser.PUndo.Last();
            var redo = action.OppositeOperation();
            this.PParser.PUndo.Remove(action);
            //if (redo.PBeforeAction != null) {
            //    redo.PBeforeAction.ResetPoint();
            //    redo.PBeforeAction.Execute();
            //}
            redo.ResetPoint();
            redo.Execute();
            //if (redo.PAfterAction != null) {
            //    redo.PAfterAction.ResetPoint();
            //    redo.PAfterAction.Execute();
            //}

            this.PParser.PRedo.Add(redo);
            if (this.PParser.PRedo.Count > this.PParser.PIEdit.GetRepealCount)
                this.PParser.PRedo.RemoveAt(0);
        }

    }
}
