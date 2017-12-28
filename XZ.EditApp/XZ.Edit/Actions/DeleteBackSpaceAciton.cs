using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Actions {
    /// <summary>
    /// 先删除在粘贴
    /// 适用与在回车自动添加开头内容
    /// </summary>
    public class DeleteBackSpaceAciton : BaseAction {
        public DeleteBackSpaceAciton(Parser paser)
            : base(paser) {
        }

        /// <summary>
        /// 要删除的字符
        /// </summary>
        public string DeleteString { get; set; }
        /// <summary>
        /// 删除的字符串宽度
        /// </summary>
        public int DeleteStringWidth { get; set; }

        public override void Execute() {
            base.Execute();
            this.Delete();
            this.BackSpace();
            this.SetSurosrPoint();
            this.PParser.PCursor.SetPosition();
            this.PParser.PIEdit.Invalidate();
        }

        private void Delete() {
            var text = this.PParser.GetLineString.Text.Remove(0, this.PParser.PCursor.CousorPointForWord.X + 1);
            var lnpID = this.PParser.GetLineString.GetLnpAndId();
            this.SetResetLineString(this.PParser.GetLineString, text);
            this.RemovePuckerLeavingOnly(lnpID, this.PParser.GetLineString);
            this.PParser.PCursor.CousorPointForEdit.X -= DeleteStringWidth;
            this.PParser.PCursor.CousorPointForWord.X -= DeleteString.Length;
        }

        private void BackSpace() {
            var bs = new BackSpaceAction(this.PParser);
            bs.BackSpace();
        }

        protected override BaseAction GetOperationAciton() {
            return new EnterAction(this.PParser);
        }
    }
}
