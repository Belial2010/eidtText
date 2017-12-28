using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.Edit.Entity;

namespace XZ.Edit.Actions {
    public class AllSelectAction : BaseAction {
        public AllSelectAction(Parser paser)
            : base(paser) {
        }

        public override void Execute() {
            base.Execute();

            var firs = this.PParser.PLineString.First();
            var firsWidth = CharCommand.GetLineStringWidth(firs, this.PParser.PIEdit.GetGraphics, this.PParser.PLanguageMode.TabSpaceCount);
            this.PParser.SetBgStartPoint(new CPoint(this.PParser.GetLeftSpace, 0, firsWidth, -1));
            var last = this.PParser.PLineString.Last();
            var lastWidth = CharCommand.GetLineStringWidth(last, this.PParser.PIEdit.GetGraphics, this.PParser.PLanguageMode.TabSpaceCount);
            this.PParser.SetBgEndPoint(new CPoint(
                 this.PParser.GetLeftSpace + lastWidth,
                 (this.PParser.PLineString.Count - 1) * FontContainer.FontHeight,
                 lastWidth,
                 last.Length - 1
                ));


            this.PParser.PCursor.CousorPointForWord.X = last.Length;
            this.PParser.PCursor.CousorPointForWord.Y = this.PParser.PLineString.Count - 1;
            this.PParser.PCursor.SetPosition(this.PParser.GetLeftSpace, (this.PParser.PLineString.Count - 1) * FontContainer.FontHeight, this.PParser.GetLeftSpace);
            this.PParser.PCursor.SetPosition();
            this.PParser.PIEdit.SetVerticalScrollValue();
            this.PParser.PIEdit.Invalidate();
        }


        public override BaseAction OppositeOperation() {
            throw new NotImplementedException();
        }
    }
}
