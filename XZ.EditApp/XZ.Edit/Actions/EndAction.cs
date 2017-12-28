using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.Edit.Entity;

namespace XZ.Edit.Actions {
    public class EndAction : BaseAction {
        public EndAction(Parser paser)
            : base(paser) {
        }
        public override void Execute() {
            base.Execute();
            var startPoint = new CPoint(
                    this.PParser.PCursor.CousorPointForEdit.X,
                    this.PParser.PCursor.CousorPointForEdit.Y,
                    this.PParser.GetLineString.Width,
                    this.PParser.PCursor.CousorPointForWord.X
                    );
            //if (this.PParser.ShiftCursorCPoint == null)
            //    this.PParser.ShiftCursorCPoint = startPoint;

            var endPoint = new CPoint(
                this.PParser.GetLineString.Width + this.PParser.GetLeftSpace,
                this.PParser.PCursor.CousorPointForEdit.Y,
                this.PParser.GetLineString.Width,
                this.PParser.GetLineString.Length
                );

            this.PParser.MouseMoveSelect(startPoint, endPoint);
            this.PParser.PCursor.CousorPointForWord.X = this.PParser.GetLineString.Length;
            this.PParser.PCursor.SetPosition(this.PParser.GetLineString.Width + this.PParser.GetLeftSpace, -1, this.PParser.GetLeftSpace);
            this.PParser.PCursor.SetPosition();
            this.PParser.PIEdit.Invalidate();
        }

        public override BaseAction OppositeOperation() {
            throw new NotImplementedException();
        }
    }
}
