using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Actions {
    public class LeftAction : BaseAction {

        public LeftAction(Parser paser)
            : base(paser) {

        }

        protected virtual bool IsClearSelect { get { return true; } }
        public override void Execute() {
            base.Execute();
            if (this.PParser.GetSelectPartPoint != null && IsClearSelect) {
                this.PParser.ClearSelect();
                this.PParser.PIEdit.Invalidate();
            }
            if (this.PParser.PCursor.CousorPointForWord.X == -1) {
                if (this.PParser.PCursor.CousorPointForWord.Y == 0)
                    return;
                this.PParser.PCursor.CousorPointForWord.Y--;
                this.PParser.PCursor.CousorPointForWord.X = this.PParser.GetLineString.Length - 1;
                this.PParser.PCursor.SetPosition(this.PParser.GetLineString.Width + this.PParser.GetLeftSpace,
                    this.PParser.PCursor.CousorPointForEdit.Y - FontContainer.FontHeight,
                    this.PParser.GetLeftSpace);
                this.PParser.PIEdit.Invalidate();
                this.PParser.PIEdit.SetVerticalScrollValue();
            } else {
                var index = this.PParser.PCursor.CousorPointForWord.X - 1;
                int width = this.PParser.GetLineStringIndexWidth(this.PParser.GetLineString, index);
                this.PParser.PCursor.CousorPointForWord.X--;
                this.PParser.PCursor.SetPosition(width + this.PParser.GetLeftSpace, -1, this.PParser.GetLeftSpace);
            }
            this.PParser.PCursor.SetPosition();

        }

        public override BaseAction OppositeOperation() {
            throw new NotImplementedException();
        }
    }
}
