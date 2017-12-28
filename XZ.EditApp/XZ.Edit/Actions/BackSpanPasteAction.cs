using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.Edit.Entity;

namespace XZ.Edit.Actions {
    /// <summary>
    /// 退格之后在粘贴
    /// </summary>
    public class BackSpanPasteAction : BaseAction {
        public BackSpanPasteAction(Parser paser)
            : base(paser) {
        }


        /// <summary>
        /// 删除的内容
        /// </summary>
        public string PDeleteString { get; set; }

        /// <summary>
        /// 自动插入的字符
        /// </summary>
        public string AutoInsertString { get; set; }

        /// <summary>
        /// 自动插入的字符的宽度
        /// </summary>
        public int AutoInsertStringWidth { get; set; }

        /// <summary>
        /// 删除部分坐标,该坐标同时也是相反操作选择的坐标
        /// </summary>
        public CPoint[] PDeletePoints { get; set; }

        public override void Execute() {
            base.Execute();
            this.Delete();
            this.BackSpace();
            this.Paste();
            this.SetSurosrPoint();
            this.SetSelectBg(this.PDeletePoints[0], this.PDeletePoints[1]);
            this.RestBgDrawPoint(this.PDeletePoints);
            this.PParser.PIEdit.Invalidate();
        }

        private void Delete() {
            if (string.IsNullOrEmpty(AutoInsertString))
                return;
            var text = this.PParser.GetLineString.Text.Remove(0, this.PParser.PCursor.CousorPointForWord.X + 1);
            var lnpID = this.PParser.GetLineString.GetLnpAndId();
            this.SetResetLineString(this.PParser.GetLineString, text);
            this.RemovePuckerLeavingOnly(lnpID, this.PParser.GetLineString);
            this.PParser.PCursor.CousorPointForEdit.X -= AutoInsertStringWidth;
            this.PParser.PCursor.CousorPointForWord.X -= AutoInsertString.Length;
        }

        private void BackSpace() {
            var backSpane = new BackSpaceAction(this.PParser);
            backSpane.SetSurosrPointLocal();
            backSpane.BackSpace();
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
            return new EnterAction(this.PParser);
        }

    }
}
