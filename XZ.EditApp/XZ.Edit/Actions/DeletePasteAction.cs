using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.Edit.Entity;

namespace XZ.Edit.Actions {

    /// <summary>
    /// 删除粘贴
    /// </summary>
    public class DeletePasteAction : DeleteLineStringAction {
        public DeletePasteAction(Parser paser)
            : base(paser) {
        }


        /// <summary>
        /// 选择的字符串
        /// </summary>
        public string PasteString { get; set; }
        
        ///// <summary>
        ///// 插入的字符
        ///// </summary>
        //public char InsertChar { get; set; }

        public override void Execute() {
            this.SetOperationAction();
            this.PParser.ClearCouple();
            this.DeletePart();           
            this.SetSurosrPoint();
            this.SetSelectBg(this.PStartForExecuteAfterShowSelectPoint,this.PEndForExecuteAfterShowSelectPoint);
            this.End();
            this.PasteAction();
            this.RestBgDrawPoint();            
            this.PParser.PIEdit.Invalidate();
        }

        public void DeletePart() {
            var text = this.PParser.GetLineString.Text.Remove(this.PParser.PCursor.CousorPointForWord.X + 1, DeleteString.Length);
            this.SetResetLineString(this.PParser.GetLineString, text);
        }

        private void End() {
            var action = this.PActionOperation as PasteAction;
            action.PIsUndoOrRedo = false;
            action.PPasteText = this.DeleteString;
        }

        private void PasteAction() {
            var paste = new PasteAction(this.PParser);
            paste.SetSurosrPointLocal();
            paste.PPasteText = this.PasteString;
            paste.IsPaste = true;
            paste.Paste();
        }

        protected override BaseAction GetOperationAciton() {
            //return base.GetOperationAciton();
            return new PasteAction(this.PParser);
        }

    }
}
