using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XZ.Edit.Actions {
    public class CutAction : BaseAction {
        public CutAction(Parser paser)
            : base(paser) {
            this.PIsCopy = true;
            this.PIsDrawBg = true;
        }

        /// <summary>
        /// 剪切的内容
        /// </summary>
        public string PCutString { get; private set; }

        public bool PIsCopy { get; set; }

        public bool PIsDrawBg { get; set; }

        public override void Execute() {
            if (this.PParser.GetSelectPartPoint != null) {
                this.PParser.PIEdit.SetChangeText();
                base.Execute();
                //if (PIsDrawBg)
                //    this.SetDrawBg();
                this.Cut();
                if (PIsCopy)
                    Clipboard.SetDataObject(PCutString, true);

                this.SetSurosrPoint();
                this.PParser.PIEdit.Invalidate();
                this.End();
                this.ChangeIncrementLine(this.GetDeleteLineCount * -1);
            } else
                this.PIsAddUndo = false;
        }

        public void Cut() {
            var selectBgs = this.GetSelectBg();
            this.PCutString = this.DeleteSelectPart(out this.PDeleteLineCount, false);
            if (PIsDrawBg && this.PActionOperation != null)
                this.SetDrawBg();
            this.PParser.ClearSelect();
            this.SetDeletePucker(selectBgs);

        }

        private void End() {
            (this.PActionOperation as PasteAction).PPasteText = this.PCutString;
        }

        protected override BaseAction GetOperationAciton() {
            return new PasteAction(this.PParser);
        }


    }
}
