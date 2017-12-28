using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XZ.Edit.Actions {
    public class CopyAction : BaseAction {
        public CopyAction(Parser paser)
            : base(paser) {
        }

        public override void Execute() {
            base.Execute();
            if (this.PParser.GetSelectPartPoint == null)
                return;

            var startPoint = this.PParser.GetSelectPartPoint[0];
            var endPoint = this.PParser.GetSelectPartPoint[1];

            var sbCopy = new StringBuilder();
            string copyText = string.Empty;
            if (startPoint.Y == endPoint.Y) {
                var copyLs = this.PParser.PLineString[startPoint.Y / FontContainer.FontHeight];
                copyText = copyLs.Text.Substring(startPoint.LineStringIndex + 1, endPoint.LineStringIndex - startPoint.LineStringIndex);
                sbCopy.AppendForPucker(copyLs, copyText,
                                        endPoint.LineStringIndex, this.PParser.PPucker, 0);
            } else {
                var startIndex = startPoint.Y / FontContainer.FontHeight;
                var count = (endPoint.Y - startPoint.Y) / FontContainer.FontHeight;
                var startLs = this.PParser.PLineString[startIndex];
                copyText = startLs.Text.Substring(Math.Min(startLs.Text.Length, startPoint.LineStringIndex + 1));
                sbCopy.AppendForPucker(startLs, copyText, this.PParser.PPucker);
                this.PParser.PPucker.GetPuckerLsText(startLs, sbCopy);
                for (var i = 1; i < count; i++) {
                    sbCopy.AppendForPucker(this.PParser.PLineString[startIndex + i], this.PParser.PPucker);
                    this.PParser.PPucker.GetPuckerLsText(this.PParser.PLineString[startIndex + i], sbCopy);
                }

                var endLs = this.PParser.PLineString[startIndex + count];
                var endText = endLs.Text.Substring(0, Math.Min(endLs.Text.Length, endPoint.LineStringIndex + 1));
                sbCopy.AppendForPucker(endLs, endText, endPoint.LineStringIndex, this.PParser.PPucker, 0);
            }
            Clipboard.SetDataObject(sbCopy.ToString(), true);
        }

    }



}
