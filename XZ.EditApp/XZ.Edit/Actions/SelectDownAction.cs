using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XZ.Edit.Entity;

namespace XZ.Edit.Actions {
    public class SelectDownAction : DownAction {
        public SelectDownAction(Parser paser)
            : base(paser) {
        }

        protected override bool IsClearSelect {
            get {
                return false;
            }
        }


        public override void Execute() {
            int startY = this.PParser.PCursor.CousorPointForEdit.Y;
            var startPoint = new CPoint(
                    this.PParser.PCursor.CousorPointForEdit.X,
                    this.PParser.PCursor.CousorPointForEdit.Y,
                    this.PParser.GetLineString.Width,
                    this.PParser.PCursor.CousorPointForWord.X
                    );
            base.Execute();
            var endPoint = new CPoint(
                this.PParser.PCursor.CousorPointForEdit.X,
                this.PParser.PCursor.CousorPointForEdit.Y,
                this.PParser.GetLineString.Width,
                this.PParser.PCursor.CousorPointForWord.X
            );
            if (this.PParser.GetSelectPartPoint != null) {
                if (startY == this.PParser.GetSelectPartPoint[1].Y) 
                    startPoint = this.PParser.GetSelectPartPoint[0];
                else {
                    startPoint = endPoint;
                    endPoint = this.PParser.GetSelectPartPoint[1];
                }
            }

            this.PParser.SetBgStartPoint(startPoint);
            this.PParser.SetBgEndPoint(endPoint);
            this.PParser.PIEdit.Invalidate();
        }

    }
}
