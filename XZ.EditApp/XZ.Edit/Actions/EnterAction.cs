using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using XZ.Edit.Entity;

namespace XZ.Edit.Actions {
    public class EnterAction : BaseAction {
        public EnterAction(Parser paser)
            : base(paser) {
            this.PIsRetraction = true;
        }

        /// <summary>
        /// 是否缩进
        /// </summary>
        public bool PIsRetraction { get; set; }

        public override void Execute() {
            base.Execute();
            if (this.PParser.GetSelectPartPoint != null) {
                #region 记忆删除字符
                this.PActionOperation = new BackSpanPasteAction(this.PParser);
                var pasteAction = this.PActionOperation as BackSpanPasteAction;
                var selectBgs = this.GetSelectBg();
                string delStr = this.DeleteSelectPart(out this.PDeleteLineCount, false);
                pasteAction.PDeleteString = delStr;
                pasteAction.PDeletePoints = this.GetSelectBg();
                this.SetDrawBgClearSelectAndPucker(selectBgs);
                #endregion
            }
            this.Enter();
            this.PParser.PIEdit.SetVerticalScrollValue();
            this.PParser.PIEdit.Invalidate();
        }



        private string pEnderString;
        private int pEnderStringWidth;

        /// <summary>
        ///回车
        /// </summary>
        public void Enter() {
            var ls = this.PParser.GetLineString;
            var isStartRange = ls.IsStartRange();
            //var nNode = this.PParser.GetLineString.PNode.Create();
            var lnpID = this.PParser.GetLineString.GetLnpAndId();
            var newLs = this.PParser.PCharFontStyle.GetLineString();
            string newText = string.Empty;
            if (this.PParser.GetLineString.Length == this.PParser.PCursor.CousorPointForWord.X + 1) {
                SetCousorAndText(this.PParser.GetLineString, ref newText);
                if (!string.IsNullOrEmpty(newText))
                    this.SetResetLineString(newLs, newText);
                else {
                    newLs.Text = string.Empty;
                    newLs.PWord = new List<Word>();
                }
            } else {
                newText = this.GetLineStringEffectualText().Substring(this.PParser.PCursor.CousorPointForWord.X + 1);
                this.PParser.GetLineString.Text = this.PParser.GetLineString.Text.Substring(0, this.PParser.PCursor.CousorPointForWord.X + 1);
                this.SetResetLineString(this.PParser.GetLineString);
                SetCousorAndText(this.PParser.GetLineString, ref newText);
                this.SetResetLineString(newLs, newText);
                this.RemovePuckerLeavingOnly(lnpID, newLs);
            }

            this.SetSurosrPoint();
            //this.End();
            this.PParser.PLineString.Insert(this.PParser.PCursor.CousorPointForWord.Y, newLs);
            this.ChangeIncrementLine(1 - this.GetDeleteLineCount, this.PParser.PCursor.CousorPointForWord.Y);
            //this.PParser.PPucker.SelectPuckerChange(this.PParser.PCursor.CousorPointForWord.Y);
        }

        /// <summary>
        /// 回车之前添加缩进字符
        /// </summary>
        /// <param name="ls"></param>
        /// <param name="newText"></param>
        private void SetCousorAndText(LineString ls, ref string newText) {
            string text = string.Empty;

            Word w = null;
            int width = 0, index = -1;
            if (PIsRetraction) {
                foreach (var word in ls.PWord) {
                    w = word;
                    if (word.PEWordType == EWordType.Tab) {
                        text += " ".PadLeft(this.PParser.PLanguageMode.TabSpaceCount, ' ');
                        width += FontContainer.GetSpaceWidth(this.PParser.PIEdit.GetGraphics) * this.PParser.PLanguageMode.TabSpaceCount;
                        index += this.PParser.PLanguageMode.TabSpaceCount;
                    } else if (word.PEWordType == EWordType.Space) {
                        text += " ";
                        width += FontContainer.GetSpaceWidth(this.PParser.PIEdit.GetGraphics);
                        index++;
                    } else
                        break;
                }
                if (!string.IsNullOrEmpty(ls.Text) && this.PParser.PLanguageMode.Retraction != null
                    && w != null && this.PParser.PLanguageMode.Retraction.Contains(w.Text)
                    ) {
                    if (this.PParser.PLanguageMode.RetractionAfterNoChar == null || (this.PParser.PLanguageMode.RetractionAfterNoChar != null &&
                        !this.PParser.PLanguageMode.RetractionAfterNoChar.Contains(newText.TrimStart().GetFirst()))
                        ) {
                        text += " ".PadLeft(this.PParser.PLanguageMode.TabSpaceCount, ' ');

                        width += FontContainer.GetSpaceWidth(this.PParser.PIEdit.GetGraphics) * this.PParser.PLanguageMode.TabSpaceCount;
                        index += this.PParser.PLanguageMode.TabSpaceCount;
                    }
                }

                #region 设置相反操作
                if (!string.IsNullOrEmpty(text)) {
                    if (this.PActionOperation is BackSpanPasteAction) {
                        var bspAction = this.PActionOperation as BackSpanPasteAction;
                        bspAction.AutoInsertString = text;
                        bspAction.AutoInsertStringWidth = width;
                    } else {
                        var dbsAction = new DeleteBackSpaceAciton(this.PParser);
                        dbsAction.DeleteString = text;
                        dbsAction.DeleteStringWidth = width;
                        this.PActionOperation = dbsAction;
                    }
                }
                #endregion

            }
            this.PParser.PCursor.CousorPointForWord.Y++;
            this.PParser.PCursor.CousorPointForWord.X = index;
            this.PParser.PCursor.SetPosition(width + this.PParser.GetLeftSpace, this.PParser.PCursor.CousorPointForEdit.Y + FontContainer.FontHeight, this.PParser.GetLeftSpace);
            this.PParser.PCursor.SetPosition();
            this.pEnderString = text;
            this.pEnderStringWidth = width;
            newText = text + newText;
        }

        /// <summary>
        /// 回车之后缩进
        /// </summary>
        private void End() {
            if (string.IsNullOrEmpty(this.pEnderString))
                return;

            var wordPoint = new Point(-1, this.PParser.PCursor.CousorPointForWord.Y);
            var editPoint = new Point(this.PParser.GetLeftSpace, this.PParser.PCursor.CousorPointForEdit.Y);
            var ssWord = new SursorSelectWord() {
                End = false,
                LeftWidth = 0,
                LeftWidthForWord = 0,
                LineIndex = 0,
                PWord = null,
                PWordIndex = 0
            };
            //var backPase = this.PActionOperation as BackSpaceAction;
            //var dele = new DeleteLineStringAction(this.PParser);
            //dele.DeleteString = this.pEnderString;
            //dele.DeleteStringWidth = this.pEnderStringWidth;
            //dele.pCousorPointForWord = wordPoint;
            //dele.pCousorPointForEdit = editPoint;
            //dele.pSursorSelectWord = ssWord;
            //backPase.PBeforeAction = dele;
            this.PActionOperation.SetSurosrPoint(wordPoint, editPoint, ssWord);
        }


        protected override BaseAction GetOperationAciton() {
            return new BackSpaceAction(this.PParser);
        }

    }
}
