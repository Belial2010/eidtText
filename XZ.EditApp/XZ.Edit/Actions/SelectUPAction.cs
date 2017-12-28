using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XZ.Edit.Entity;

namespace XZ.Edit.Actions {
    public class SelectUPAction : UPAction {
        public SelectUPAction(Parser paser)
            : base(paser) {
        }

        protected override bool IsClearSelect {
            get {
                return false;
            }
        }


        public override void Execute() {
            int startY = this.PParser.PCursor.CousorPointForEdit.Y;
            var endPoint = new CPoint(
                    this.PParser.PCursor.CousorPointForEdit.X,
                    this.PParser.PCursor.CousorPointForEdit.Y,
                    this.PParser.GetLineString.Width,
                    this.PParser.PCursor.CousorPointForWord.X
                    );
            base.Execute();
            var startPoint = new CPoint(
                this.PParser.PCursor.CousorPointForEdit.X,
                    this.PParser.PCursor.CousorPointForEdit.Y,
                    this.PParser.GetLineString.Width,
                    this.PParser.PCursor.CousorPointForWord.X
                );
            if (this.PParser.GetSelectPartPoint != null) {
                if (startY == this.PParser.GetSelectPartPoint[0].Y)
                    endPoint = this.PParser.GetSelectPartPoint[1];
                else {
                    endPoint = startPoint.Create();
                    startPoint = this.PParser.GetSelectPartPoint[0];                    
                }
            }

            this.PParser.SetBgStartPoint(startPoint);
            this.PParser.SetBgEndPoint(endPoint);

            this.PParser.PIEdit.Invalidate();
        }

        public override BaseAction OppositeOperation() {
            return base.OppositeOperation();
        }



    }
}
