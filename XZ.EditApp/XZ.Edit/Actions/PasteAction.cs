using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XZ.Edit.Entity;

namespace XZ.Edit.Actions {
    public class PasteAction : BaseAction {
        public PasteAction(Parser paser)
            : base(paser) {
        }

        /// <summary>
        /// 要粘贴的内容
        /// </summary>
        public string PPasteText { get; set; }

        public bool IsPaste { get; set; }

        private string GetPasteText() {
            if (this.PPasteText != null)
                return this.PPasteText;
            if (Clipboard.ContainsText())
                return Clipboard.GetText();
            return null;
        }

        public override void Execute() {
            base.Execute();
            this.Paste();
            this.SetSurosrPoint();
            this.RestBgDrawPoint();
            this.PParser.PCursor.SetPosition();
            this.PParser.PIEdit.SetVerticalScrollValue();
            this.PParser.PIEdit.Invalidate();
        }

        /// <summary>
        /// 粘贴
        /// </summary>
        public void Paste() {
            string value = GetPasteText();
            if (string.IsNullOrEmpty(value))
                return;
            this.PParser.PIEdit.SetChangeText();
            int startY = this.PParser.PCursor.CousorPointForWord.Y;
            this.PStartLineY = startY;
            if (this.PParser.GetSelectPartPoint != null) {
                //var pasteAction = this.DeletePartBefore();
                this.PActionOperation = this.GetCutPasteAction();
                var pasteAction = this.PActionOperation as CutPasteAction;
                var selectBgs = this.GetSelectBg();
                var delStr = this.DeleteSelectPart(out this.PDeleteLineCount, false);

                //pasteAction.PPasteText = delStr;
                pasteAction.PDeleteString = delStr;
                pasteAction.PDeletePoints = this.GetSelectBg();
                this.SetDrawBgClearSelectAndPucker(selectBgs);
            }
            #region 设置开始坐标
            int startBgY = this.PParser.PCursor.CousorPointForWord.Y;
            int startWordIndex = this.PParser.PCursor.CousorPointForWord.X;
            if (!this.IsPaste) {
                this.SetSelectBg(new CPoint(
                     this.PParser.PCursor.CousorPointForEdit.X,
                     this.PParser.PCursor.CousorPointForEdit.Y,
                     this.PParser.GetLineString.Width,
                     this.PParser.PCursor.CousorPointForWord.X
                    ), null);
            }
            #endregion
            #region 粘贴内容
            var lnpID = this.PParser.GetLineString.GetLnpAndId();
            var leavingString = this.GetLineStringEffectualText().Substring(this.PParser.PCursor.CousorPointForWord.X + 1);
            var array = value.Split(CharCommand.Char_Newline);
            if (array.Length == 1) {
                var line = array[0].TrimEnd(CharCommand.Char_Enter);
                this.PParser.GetLineString.Text = this.PParser.GetLineString.Text.Substring(0, this.PParser.PCursor.CousorPointForWord.X + 1) + line + leavingString;
                //this.SetResetLineString(this.PParser.GetLineString);
                this.SetResetLineString(this.PParser.GetLineString);
                this.RemovePuckerLeavingOnly(lnpID, this.PParser.GetLineString);
                this.PParser.PCursor.CousorPointForWord.X += line.Length;
                int x = this.PParser.GetLineStringIndexWidth(this.PParser.GetLineString, this.PParser.PCursor.CousorPointForWord.X);
                this.PParser.PCursor.SetPosition(x + this.PParser.GetLeftSpace, -1, this.PParser.GetLeftSpace);

            } else {
                this.PParser.PPucker.InitSelectPuckerStartEndY();
                #region 展开
                if (this.PParser.GetLineString.IsFurl()) {
                    var puckerAction = new PuckerAction(this.PParser);
                    puckerAction.SetPuckerIndexY(this.PParser.PCursor.CousorPointForWord.Y);
                    puckerAction.ClickPucker();
                    this.PParser.AddAction(puckerAction);
                }
                #endregion
                for (var i = 0; i < array.Length; i++) {
                    var line = array[i].TrimEnd(CharCommand.Char_Enter);
                    if (i == 0) {
                        this.PParser.GetLineString.Text = this.PParser.GetLineString.Text.Substring(0, Math.Min(this.PParser.GetLineString.Text.Length, this.PParser.PCursor.CousorPointForWord.X + 1)) + line;
                        this.SetResetLineString(this.PParser.GetLineString);
                        continue;
                    }
                    var ls = this.PParser.PCharFontStyle.GetLineString();
                    if (i == array.Length - 1) {
                        line += leavingString;
                        this.SetResetLineString(ls, line);
                        this.PParser.PCursor.CousorPointForWord.X = ls.Text.Length - leavingString.Length - 1;
                        this.RemovePuckerLeavingOnly(lnpID, ls);
                    } else
                        this.SetResetLineString(ls, line);
                    this.PParser.PLineString.Insert(this.PParser.PCursor.CousorPointForWord.Y + i, ls);
                }
                var lastIndex = this.PParser.PCursor.CousorPointForWord.Y + array.Length - 1;
                this.PParser.PCursor.CousorPointForWord.Y += array.Length - 1;
                int x = this.PParser.GetLineStringIndexWidth(this.PParser.PLineString[lastIndex], this.PParser.PCursor.CousorPointForWord.X);
                this.PParser.PCursor.SetPosition(x + this.PParser.GetLeftSpace, this.PParser.PCursor.CousorPointForEdit.Y + (array.Length - 1) * FontContainer.FontHeight, this.PParser.GetLeftSpace);
                startY += array.Length - 1;
            }
            #endregion
            #region 设置结束坐标
            var startLs = this.PParser.PLineString[startBgY];
            if (!string.IsNullOrEmpty(startLs.Text) && !this.IsPaste)
                this.SetSelectGgLineWidth(this.PParser.GetLineStringIndexWidth(startLs, startLs.Length));
            if (!this.IsPaste) {
                this.SetSelectBg(null, new CPoint(
                        this.PParser.PCursor.CousorPointForEdit.X,
                        this.PParser.PCursor.CousorPointForEdit.Y,
                        this.PParser.GetLineString.Width,
                        this.PParser.PCursor.CousorPointForWord.X
                       ));
            }
            #endregion

            this.ChangeIncrementLine(array.Length - 1 - this.GetDeleteLineCount, startY);
        }

        ///// <summary>
        ///// 删除内容之前将数据保存
        ///// </summary>
        //private PasteAction DeletePartBefore() {
        //    var paste = new PasteAction(this.PParser);
        //    paste.SetSurosrPointLocal(this.PParser.GetSelectPartPoint[0]);
        //    paste.SetDrawBgLocal();
        //    this.PActionOperation.PAfterAction = paste;
        //    return paste;
        //}

        public void SetOAciton() {
            base.Execute();
        }

        private CutPasteAction GetCutPasteAction() {
            return new CutPasteAction(this.PParser);
        }

        protected override BaseAction GetOperationAciton() {
            var cut = new CutAction(this.PParser);
            cut.PIsCopy = false;
            cut.PIsDrawBg = this.PIsUndoOrRedo;
            return cut;

            //var cut = new CutPasteAction(this.PParser);
            //return cut;
        }

    }
}
