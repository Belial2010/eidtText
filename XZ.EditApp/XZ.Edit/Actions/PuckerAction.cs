using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.Edit.Entity;

namespace XZ.Edit.Actions {
    public class PuckerAction : BaseAction {
        public PuckerAction(Parser paser)
            : base(paser) {
        }

        private int pY;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ls"></param>
        /// <param name="y"></param>
        public void Pucker(int y) {
            this.pY = y;
            this.Execute();
        }

        public override void Execute() {
            base.Execute();
            this.SetDrawBg();
            //this.SetSelectBg(this.PStartBGDrawPoint, this.PEndBgDrawPoint);
            this.ClickPucker();
            this.SetSurosrPoint();
            this.RestBgDrawPoint();
            //this.sel
            this.SetSelectBg();
            this.PParser.PCursor.SetPosition();
            this.PParser.PIEdit.Invalidate();
        }

        public void SetPuckerIndexY(int y) {
            this.pY = y;
        }

        public void ClickPucker() {
            if (this.PActionOperation == null)
                this.SetOperationAction();
            this.PParser.PPucker.InitSelectPuckerStartEndY();
            var ls = this.PParser.PLineString[this.pY];
            this.PParser.PPucker.ClickPuckerUnfurl(ls, this.pY);
        }

        protected override BaseAction GetOperationAciton() {
            var action = new PuckerAction(this.PParser);
            action.pY = this.pY;
            return action;
        }

    }
}
