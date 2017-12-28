using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.Edit.Entity;

namespace XZ.Edit.Actions {
    /// <summary>
    /// 缩进
    /// </summary>
    public class RetractAction : BaseAction {

        public RetractAction(Parser paser)
            : base(paser) {
            this.PRetractType = RetractType.Add;
            this.IsValid = true;
        }

        /// <summary>
        /// 是否验证有效性
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 缩进类型
        /// </summary>
        public RetractType PRetractType { get; set; }

        public override void Execute() {
            base.Execute();
            //this.SetDrawBg();
            this.SetSelectBg();
            this.SetSurosrPoint();
            this.Init();

            //this.RestBgDrawPoint();
            this.PParser.PCursor.SetPosition();
            this.PParser.PIEdit.Invalidate();
        }

        public void Init() {
            if (!Valid())
                return;
            if (this.PRetractType == RetractType.Add)
                this.InitAdd();
            else
                this.InitLessen();
        }

        private bool Valid() {
            if (!IsValid)
                return true;
            return this.PParser.GetSelectPartPoint != null && this.PParser.GetSelectPartPoint[0].Y != this.PParser.GetSelectPartPoint[1].Y;
        }

        /// <summary>
        /// 减少
        /// </summary>
        private void InitLessen() {
            int top = this.PParser.GetSelectPartPoint[0].Y;
            int firstWidth = 0, endWidth = 0, firstIndex = 0, endIndex = 0;
            for (var i = top; i <= this.PParser.GetSelectPartPoint[1].Y; i = i + FontContainer.FontHeight) {
                var ls = this.PParser.PLineString[i / FontContainer.FontHeight];
                string leftString = null;
                int leftWidth = 0;
                var wi = 0;
                #region 循环Word
                int count = Math.Min(ls.PWord.Count, this.PParser.PLanguageMode.TabSpaceCount);
                for (; wi < count; wi++) {
                    var w = ls.PWord[0];
                    if (w.PEWordType == EWordType.Tab) {
                        if (wi != 0)
                            break;
                        leftString = w.Text;
                        ls.PWord.Remove(w);
                        leftWidth = w.Width;
                        break;
                    } else if (w.PEWordType == EWordType.Space) {
                        leftString += w.Text;
                        ls.PWord.Remove(w);
                        leftWidth += w.Width;
                    } else
                        break;
                }

                if (i == top) {
                    firstWidth = leftWidth;
                    firstIndex = wi;
                } else if (i == this.PParser.GetSelectPartPoint[1].Y) {
                    endWidth = leftWidth;
                    endIndex = wi;
                }
                #endregion
                if (wi > 0) {
                    ls.SetText(ls.Text.Substring(wi, ls.Text.Length - wi));
                    ls.Width -= leftWidth;
                }
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        private void InitAdd() {
            int top = this.PParser.GetSelectPartPoint[0].Y;
            string spaceString = " ".PadLeft(this.PParser.PLanguageMode.TabSpaceCount);
            int width = FontContainer.GetSpaceWidth(this.PParser.PIEdit.GetGraphics);
            while (top <= this.PParser.GetSelectPartPoint[1].Y) {
                var ls = this.PParser.PLineString[top / FontContainer.FontHeight];
                ls.SetText(ls.Text.Insert(0, spaceString));
                ls.Width += width * this.PParser.PLanguageMode.TabSpaceCount;
                for (var i = 0; i < this.PParser.PLanguageMode.TabSpaceCount; i++)
                    ls.PWord.Insert(0, new Word() {
                        Text = " ",
                        PEWordType = EWordType.Space,
                        Width = width,
                    });
                top += FontContainer.FontHeight;
            }

            width = width * this.PParser.PLanguageMode.TabSpaceCount;
            this.PParser.GetSelectPartPoint[0].LineStringIndex += this.PParser.PLanguageMode.TabSpaceCount;
            this.PParser.GetSelectPartPoint[0].LineWidth += width;
            this.PParser.GetSelectPartPoint[1].LineStringIndex += this.PParser.PLanguageMode.TabSpaceCount;
            this.PParser.GetSelectPartPoint[1].LineWidth += width;
            this.PParser.GetSelectPartPoint[1].X += width;
        }

        protected override BaseAction GetOperationAciton() {
            var action = new RetractAction(this.PParser);
            action.PRetractType = this.PRetractType == RetractType.Add ? RetractType.Lessen : RetractType.Add;
            return action;
        }

    }

    /// <summary>
    /// 缩进类型
    /// </summary>
    public enum RetractType {
        /// <summary>
        /// 减少
        /// </summary>
        Lessen,
        /// <summary>
        /// 增加
        /// </summary>
        Add
    }

}
