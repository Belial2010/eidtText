using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.Edit.Entity;

namespace XZ.Edit.Actions {
    public class DeleteLineStringAction : BaseAction {
        public DeleteLineStringAction(Parser paser)
            : base(paser) {
        }


        /// <summary>
        /// 要删除的字符串
        /// </summary>
        public string DeleteString { get; set; }

        /// <summary>
        /// 插入字符串的宽度
        /// </summary>
        public int DeleteStringWidth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public override void Execute() {
            base.Execute();
            var lnpID = this.PParser.GetLineString.GetLnpAndId();
            var text = this.GetLineStringEffectualText().Remove(this.PParser.PCursor.CousorPointForWord.X + 1, DeleteString.Length);
            this.SetResetLineString(this.PParser.GetLineString, text);
            this.RemovePuckerLeavingOnly(lnpID, this.PParser.GetLineString);
            this.PParser.PCursor.SetPosition();
            this.SetSurosrPoint();
            this.PParser.PIEdit.Invalidate();
            this.End();
        }

        private void End() {
            var paste = this.PActionOperation as PasteAction;
            paste.PIsUndoOrRedo = false;
            paste.PPasteText = this.DeleteString;
        }

        protected override BaseAction GetOperationAciton() {
            return new PasteAction(this.PParser);
        }
    }
}
