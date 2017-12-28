using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Actions {
    public class RedoAction : BaseAction {
        public RedoAction(Parser paser)
            : base(paser) {
        }

        public override void Execute() {
        //Start:
            if (this.PParser.PRedo.Count == 0)
                return;
            this.PParser.ClearSelect();
            var action = this.PParser.PRedo.Last();
            var undo = action.OppositeOperation();
            this.PParser.PRedo.Remove(action);
            if (undo is NoneAction) {
                undo.ResetPoint();
                var nAction = undo as NoneAction;
                if (nAction.RedoExecute != null)
                    nAction.RedoExecute(undo);
                this.PParser.PIEdit.Invalidate();
                return;
            }
            //if (undo.PAfterAction != null) {
            //    undo.PAfterAction.ResetPoint();
            //    undo.PAfterAction.Execute();
            //}
            undo.ResetPoint();
            undo.Execute();
            //if (undo.PBeforeAction != null) {
            //    undo.PBeforeAction.ResetPoint();
            //    undo.PBeforeAction.Execute();
            //}

            this.PParser.PUndo.Add(undo);
            if (this.PParser.PUndo.Count > this.PParser.PIEdit.GetRepealCount)
                this.PParser.PUndo.RemoveAt(0);
        }

    }
}
