using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Actions {
    public class UPAction : BaseAction {
        public UPAction(Parser paser)
            : base(paser) {
        }
        protected virtual bool IsClearSelect { get { return true; } }
        public override void Execute() {
            if (IsClearSelect)
                this.PParser.ClearSelect();

            if (this.PParser.UpDownLineStringDefaultIndex == -1)
                this.PParser.UpDownLineStringDefaultIndex = this.PParser.PCursor.CousorPointForWord.X;
            if (this.PParser.PCursor.CousorPointForWord.Y == 0) {
                //this.PParser.PCursor.CousorPointForWord.X = 
                return;
            }
            this.PParser.PCursor.CousorPointForWord.Y--;
            int x = 0;
            if (this.PParser.GetLineString.Length > this.PParser.UpDownLineStringDefaultIndex) {
                this.PParser.PCursor.CousorPointForWord.X = this.PParser.UpDownLineStringDefaultIndex;
                x = this.PParser.GetLineStringIndexWidth(this.PParser.GetLineString, this.PParser.PCursor.CousorPointForWord.X);
            } else {
                this.PParser.PCursor.CousorPointForWord.X = this.PParser.GetLineString.Length - 1;
                x = this.PParser.GetLineString.Width;
            }
            int y = this.PParser.PCursor.CousorPointForWord.Y * FontContainer.FontHeight;


            this.PParser.PCursor.SetPosition(x + this.PParser.GetLeftSpace, y, this.PParser.GetLeftSpace);
            this.PParser.PCursor.SetPosition();

            this.PParser.PIEdit.Invalidate();
            if (y < this.PParser.PIEdit.GetVerticalScrollValue)
                this.PParser.PIEdit.SetVerticalScrollValue();



        }

    }
}
