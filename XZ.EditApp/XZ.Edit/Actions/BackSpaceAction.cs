using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.Edit.Entity;

namespace XZ.Edit.Actions {
    public class BackSpaceAction : BaseAction {
        private char pChar;
        public BackSpaceAction(Parser paser)
            : base(paser) {
        }

        private EBackSpaceType pEBackSpaceType;

        public override void Execute() {
            //base.Execute();
            //this.SetNowLNProperty();
            this.PParser.ClearCouple();
            this.BackSpace();
            this.PParser.PIEdit.Invalidate();
        }

        public void BackSpace() {
            if (this.PParser.GetSelectPartPoint == null)
                BackSpaceChar();
            else {
                this.pEBackSpaceType = EBackSpaceType.Select;
                this.SetOperationAction();
                var selectBgs = this.GetSelectBg();
                string backSpaceStr = this.DeleteSelectPart(out this.PDeleteLineCount, false);
                this.SetDrawBgClearSelectAndPucker(selectBgs);
                (this.PActionOperation as PasteAction).PPasteText = backSpaceStr;
                this.SetSurosrPoint();
                this.ChangeIncrementLine(this.GetDeleteLineCount * -1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void BackSpaceChar() {
            LineString lineString = null;
            string lineText = string.Empty;
            var lnpID = this.PParser.GetLineString.GetLnpAndId();
            if (this.PParser.PCursor.CousorPointForWord.X < 0) {
                if (this.PParser.PCursor.CousorPointForWord.Y == 0) {
                    this.PIsAddUndo = false;
                    return;
                }
                this.pEBackSpaceType = EBackSpaceType.Enter;
                this.SetOperationAction();
                string leavings = this.PParser.GetLineString.Text;
                var nowLine = this.PParser.GetLineString;
                this.PParser.PLineString.RemoveAt(this.PParser.PCursor.CousorPointForWord.Y);
                this.PParser.PCursor.CousorPointForWord.Y--;
                this.PParser.PCursor.CousorPointForEdit.Y -= FontContainer.FontHeight;
                if (this.PParser.PCursor.CousorPointForWord.Y < 0) {
                    this.PParser.PCursor.CousorPointForWord.Y = 0;
                    return;
                }
                lineString = this.PParser.GetLineString;
                this.MergeLineString(lineString, nowLine);
                this.ChangeIncrementLine(-1);
            } else {
                pEBackSpaceType = EBackSpaceType.Char;
                this.SetOperationAction();
                lineString = this.PParser.GetLineString;
                this.pChar = lineString.Text[this.PParser.PCursor.CousorPointForWord.X];
                lineText = this.GetLineStringEffectualText(lineString).Remove(this.PParser.PCursor.CousorPointForWord.X, 1);

                int with = CharCommand.GetCharWidth(this.PParser.PIEdit.GetGraphics, this.pChar.ToString(), FontContainer.DefaultFont);
                this.PParser.PCursor.XForLeft -= with;
                this.PParser.PCursor.CousorPointForEdit.X -= with;
                this.PParser.PCursor.CousorPointForWord.X -= 1;
                this.SetResetLineString(lineString, lineText);
                this.RemovePuckerLeavingOnly(lnpID, lineString);
                this.EndInsertChar();
            }
            this.SetSurosrPoint();
            this.PParser.PCursor.SetPosition();
        }

        /// <summary>
        /// 合并行
        /// </summary>
        /// <param name="upLs"></param>
        /// <param name="nowLs"></param>
        private void MergeLineString(LineString upLs, LineString nowLs) {
            LineString changeLine = upLs;
            if (upLs.IsFurl()) {
                var hideArray = this.PParser.PPucker.PDictPuckerList[upLs.ID];
                changeLine = hideArray.Last();
                //upLs.PLNProperty.IsFurl = false;
                this.PParser.PPucker.ClickPucker(upLs, this.PParser.PCursor.CousorPointForWord.Y);
                this.PParser.PCursor.CousorPointForWord.Y += hideArray.Length;
                this.PParser.PCursor.CousorPointForEdit.Y += hideArray.Length * FontContainer.FontHeight;
            }
            if (nowLs.IsFurl()) {
                if (changeLine.IsEndRange()) {
                    //nowLs.PLNProperty.IsFurl = false;
                    this.PParser.PPucker.ClickPucker(nowLs, this.PParser.PCursor.CousorPointForWord.Y);
                } else {
                    MergeLineStringChangeXY(changeLine);
                    changeLine.Text += this.GetLineStringEffectualText(nowLs);
                    this.SetResetLineString(changeLine);
                    this.RemovePuckerLeavingOnly(nowLs.GetLnpAndId(), changeLine);
                    return;
                }
            }
            MergeLineStringChangeXY(changeLine);
            changeLine.Text += nowLs.Text;
            this.SetResetLineString(changeLine);
        }
        
        /// <summary>
        /// 设置行属性
        /// </summary>
        /// <param name="defLNP"></param>
        /// <param name="newLs"></param>
        /// <param name="y"></param>
        private void SetLineStringLNP() {
            //this.PParser.ClearStartEndPostion(this.PChangeIncStartY, -1);
            //this.PParser.IncrementLineNum(1);
            //this.PParser.PPucker.AddLineIndex(this.PParser.PCursor.CousorPointForWord.Y, -1);
        }

        /// <summary>
        /// 
        /// </summary>
        private void EndInsertChar() {
            var insert = this.PActionOperation as InsertAction;
            insert.PInsertChar = this.pChar;
        }

        protected override BaseAction GetOperationAciton() {
            switch (this.pEBackSpaceType) {
                case EBackSpaceType.Char:
                    return new InsertAction(this.PParser);
                case EBackSpaceType.Enter:
                    return new EnterAction(this.PParser) {
                        PIsRetraction = this.PIsUndoOrRedo
                    };
                case EBackSpaceType.Select:
                    return new PasteAction(this.PParser);
                default:
                    throw new Exception("退格无效");
            }
        }
    }

    public enum EBackSpaceType {
        Select,
        Enter,
        Char
    }
}
