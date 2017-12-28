using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.Edit.Entity;

namespace XZ.Edit.Actions {
    /// <summary>
    /// 剪切之后再粘贴。    
    /// 因为要又剪切部分
    /// </summary>
    public class CutPasteAction : BaseAction {

        public CutPasteAction(Parser paser)
            : base(paser) {
        }

        /// <summary>
        /// 剪切的内容
        /// </summary>
        private string pCutString { get; set; }

        /// <summary>
        /// 删除的内容
        /// </summary>
        public string PDeleteString { get; set; }

        /// <summary>
        /// 删除部分坐标,该坐标同时也是相反操作选择的坐标
        /// </summary>
        public CPoint[] PDeletePoints { get; set; }

        public override void Execute() {
            base.Execute();
            this.SetSurosrPoint();
            this.SetSelectBg(this.PStartBgPoint, this.PEndBgPoint);
            this.Cut();
            this.Paste();
            //this.SetSurosrPoint();
            this.SetSelectBg(this.PDeletePoints[0], this.PDeletePoints[1]);
            this.RestBgDrawPoint(this.PDeletePoints);
            this.PParser.PCursor.SetPosition();
            this.PParser.PIEdit.Invalidate();
            this.End();
        }

        private void End() {
            if(this.pCutString == null)
                return;
            var action = this.PActionOperation as PasteAction;
            if (action.PPasteText == null)
                action.PPasteText = this.pCutString;
        }

        /// <summary>
        /// 剪切原来粘贴的内容
        /// </summary>
        private void Cut() {
            var cut = new CutAction(this.PParser);
            cut.SetSelectBgLocal(this.PStartBgPoint, this.PEndBgPoint);
            cut.RestBgPoint();
            cut.Cut();
            this.pCutString = cut.PCutString;
        }

        /// <summary>
        /// 粘贴
        /// </summary>
        private void Paste() {
            var paste = new PasteAction(this.PParser);
            paste.SetSurosrPointLocal();
            //paste.SetDrawBgLocal(this.PDeletePoints);
            paste.PPasteText = this.PDeleteString;
            paste.IsPaste = true;
            paste.Paste();
        }

        protected override BaseAction GetOperationAciton() {
            //return base.GetOperationAciton();
            var pasteAction = new PasteAction(this.PParser);
            pasteAction.PPasteText = this.pCutString;
            //pasteAction.set
            return pasteAction;
        }

    }
}
