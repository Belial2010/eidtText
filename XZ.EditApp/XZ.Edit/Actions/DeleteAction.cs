using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XZ.Edit.Entity;

namespace XZ.Edit.Actions {
    public class DeleteAction : BaseAction {
        public DeleteAction(Parser paser)
            : base(paser) {
        }

        private EDeleteType pType;

        /// <summary>
        /// 删除的字符串
        /// </summary>
        private string pDelStr { get; set; }

        public override void Execute() {
            this.PParser.PIEdit.SetChangeText();
            if (this.PParser.GetSelectPartPoint == null)
                DelectChar();
            else {
                this.PParser.PIEdit.SetChangeText();
                this.pType = EDeleteType.Select;
                base.Execute();
                //this.SetSelectBg();
                var selectBgs = this.GetSelectBg();
                pDelStr = this.DeleteSelectPart(out this.PDeleteLineCount,false);
                this.SetDrawBgClearSelectAndPucker(selectBgs);

                this.SetSurosrPoint();
                this.End();
                this.ChangeIncrementLine(this.GetDeleteLineCount * -1);
            }
            this.PParser.PIEdit.Invalidate();

        }

        /// <summary>
        /// 删除字符
        /// </summary>
        private void DelectChar() {
            if (this.PParser.PCursor.CousorPointForWord.X + 1 >= this.PParser.GetLineString.Text.Length) {
                if (this.PParser.PCursor.CousorPointForWord.Y >= this.PParser.PLineString.Count - 1) {
                    this.PIsAddUndo = false;
                    return;
                }
                this.pType = EDeleteType.Enter;
                base.Execute();
                this.MergeLineString();
                this.ChangeIncrementLine(-1);

            } else {
                var lnpID = this.PParser.GetLineString.GetLnpAndId();
                this.pType = EDeleteType.Char;
                base.Execute();
                var deleteChar = this.PParser.GetLineString.Text[this.PParser.PCursor.CousorPointForWord.X + 1];
                var text = this.GetLineStringEffectualText().Remove(this.PParser.PCursor.CousorPointForWord.X + 1, 1);
                //this.ResetLineString(this.PParser.GetLineString, text);
                this.SetResetLineString(this.PParser.GetLineString, text);
                this.RemovePuckerLeavingOnly(lnpID, this.PParser.GetLineString);
                (this.PActionOperation as InsertAction).PInsertChar = deleteChar;
            }
            this.SetSurosrPoint();
        }

        /// <summary>
        /// 合并
        /// </summary>
        /// <param name="upLs"></param>
        /// <param name="nowLs"></param>
        private void MergeLineString() {
            var nowLs = this.PParser.GetLineString;
            LineString nextLS = null;
            if (nowLs.IsFurl()) {
                var hideArray = this.PParser.PPucker.PDictPuckerList[nowLs.ID];
                nowLs.PLNProperty.IsFurl = false;
                this.PParser.PPucker.ClickPucker(nowLs, this.PParser.PCursor.CousorPointForWord.Y);
                this.PParser.PCursor.SetPosition();
            }
            nextLS = this.PParser.PLineString[this.PParser.PCursor.CousorPointForWord.Y + 1];
            this.PParser.PLineString.RemoveAt(this.PParser.PCursor.CousorPointForWord.Y + 1);
            if (nextLS.IsFurl()) {
                if (nowLs.IsEndRange()) {
                    nextLS.PLNProperty.IsFurl = false;
                    this.PParser.PPucker.ClickPucker(nextLS, this.PParser.PCursor.CousorPointForWord.Y + 1);
                } else {
                    MergeLineStringChangeXY(nowLs);
                    nowLs.Text += this.GetLineStringEffectualText(nextLS);
                    this.SetResetLineString(nowLs);
                    this.RemovePuckerLeavingOnly(nextLS.GetLnpAndId(), this.PParser.GetLineString);
                    return;
                }
            }

            MergeLineStringChangeXY(nowLs);
            nowLs.Text += nextLS.Text;
            this.SetResetLineString(nowLs);
        }

        private void End() {
            (this.PActionOperation as PasteAction).PPasteText = this.pDelStr;
        }

        protected override BaseAction GetOperationAciton() {
            switch (this.pType) {
                case EDeleteType.Select:
                    return new PasteAction(this.PParser);
                case EDeleteType.Char:
                    return new InsertAction(this.PParser);
                default:
                    return new EnterAction(this.PParser);
            }
        }
    }

    public enum EDeleteType {
        Char,
        Enter,
        Select
    }
}
