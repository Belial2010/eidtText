using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.Edit.Entity;
using XZ.Edit.Interfaces;

namespace XZ.Edit.Actions {
    public class InsertAction : BaseAction {
        //private char pChar;
        public InsertAction(Parser paser)
            : base(paser) {
        }

        /// <summary>
        /// 插入字符
        /// </summary>
        public char PInsertChar { get; set; }

        /// <summary>
        /// 插入的字符串
        /// </summary>
        public string PInsertString { get; set; }

        /// <summary>
        /// 字符的宽度
        /// </summary>
        public int PCharWidth { get; set; }

        public override void Execute() {
            this.AddChar(this.PInsertChar);
        }

        /// <summary>
        /// 删除的字符串
        /// </summary>
        private string pDeleteString;
        private bool isDelectString;
        /// <summary>
        /// 添加字符串
        /// </summary>
        /// <param name="c"></param>
        public void AddChar(char c) {
            //this.SetSurosrPointLocal();
            this.PInsertChar = c;

            if (this.PParser.GetSelectPartPoint != null) {
                this.isDelectString = true;
                base.Execute();
                
                #region 记忆删除字符
                var selectBgs = this.GetSelectBg();
                this.pDeleteString = this.DeleteSelectPart(out this.PDeleteLineCount, false);
                this.SetDrawBgClearSelectAndPucker(selectBgs);
                this.ChangeIncrementLine(this.GetDeleteLineCount * -1);
                #endregion
            }
            #region 插入单个字符串
            if (this.PActionOperation == null)
                base.Execute();
            this.SetSurosrPoint();
            var lnpID = this.PParser.GetLineString.GetLnpAndId();
            string insertString = c.ToString();
            if (c == CharCommand.Char_Tab)
                insertString = " ".PadLeft(this.PParser.PLanguageMode.TabSpaceCount, ' ');
            this.PInsertString = insertString;
            var text = this.GetLineStringEffectualText();
            text = text.Insert(this.PParser.PCursor.CousorPointForWord.X + 1, insertString);
            this.SetResetLineString(this.PParser.GetLineString, text);
            this.RemovePuckerLeavingOnly(lnpID, this.PParser.GetLineString);
            
            int with = CharCommand.GetCharWidth(this.PParser.PIEdit.GetGraphics, insertString, FontContainer.DefaultFont);
            this.PCharWidth = with;

            this.PParser.PCursor.CousorPointForEdit.X += with;
            this.PParser.PCursor.CousorPointForWord.X += insertString.Length;
            if (this.PParser.PCursor.CousorPointForEdit.X > this.PParser.PIEdit.GetWidth - 20) {
                if (this.PParser.PCursor.CousorPointForEdit.X > this.PParser.GetMaxWidth + this.PParser.GetLeftSpace)
                    this.PParser.PIEdit.SetMaxScollMaxWidth(this.PParser.PCursor.CousorPointForEdit.X);

                this.PParser.PCursor.CousorPointForEdit.X -= with;
                this.PParser.PIEdit.SetHorizontalScrollValue(with + this.PParser.PIEdit.GetHorizontalScrollValue, 1);
            }
            this.PParser.PCursor.SetPosition(this.PParser.PCursor.CousorPointForEdit.X, -1, this.PParser.GetLeftSpace);
            this.PParser.PCursor.SetPosition();
            //this.PParser.ResetLineLNPAndClearPucker(formerly, this.PParser.GetLineString);
            this.PParser.PIEdit.Invalidate();
            this.End();
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        private void End() {

            if (this.isDelectString) {
                var dpAction = (this.PActionOperation as DeletePasteAction);
                dpAction.DeleteString = this.PInsertString;
                dpAction.DeleteStringWidth = this.PCharWidth;
                dpAction.PasteString = this.pDeleteString;
            } else {
                var deleAction = (this.PActionOperation as DeleteLineStringAction);
                deleAction.DeleteString = this.PInsertString;
                deleAction.DeleteStringWidth = this.PCharWidth;
            }

        }


        protected override BaseAction GetOperationAciton() {
            if (this.isDelectString)
                return new DeletePasteAction(this.PParser);
            else
                return new DeleteLineStringAction(this.PParser);
        }

        ///// <summary>
        ///// 插入更多字符
        ///// </summary>
        ///// <param name="c"></param>
        //public void InsertMoreChar(char c, int charWidth) {
        //    if (this.PActionOperation is BackSpaceAction) {
        //        var delLineString = new DeleteLineStringAction(this.PParser);
        //        delLineString.DeleteString = this.PInsertChar.ToString();
        //        this.PActionOperation = delLineString;
        //    }
        //}
    }
}
