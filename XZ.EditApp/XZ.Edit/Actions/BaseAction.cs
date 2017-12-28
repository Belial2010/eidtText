using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XZ.Edit.Entity;
using XZ.Edit.Interfaces;

namespace XZ.Edit.Actions {
    public abstract class BaseAction {
        protected Parser PParser { get; set; }

        //public BaseAction

        public bool PIsAddUndo { get; set; }

        //public bool PIsAddRedo { get; set; }

        public BaseAction(Parser parser) {
            this.PParser = parser;
            this.PIsAddUndo = true;
            this.PIsUndoOrRedo = true;
            //this.PIsAddRedo = true;
        }

        /// <summary>
        /// 一个相反的操作
        /// </summary>
        public BaseAction PActionOperation { get; set; }


        #region 保留的坐标

        /// <summary>
        /// 光标在文本中的索引位置
        /// </summary>
        public Point pCousorPointForWord;

        /// <summary>
        /// 光标位置 x 到x轴坐标，y 到顶部0位置坐标（不是到编辑器区域顶部）
        /// </summary>
        public Point pCousorPointForEdit;
        public SursorSelectWord pSursorSelectWord;

        /// <summary>
        /// 调用Execute之前，需要重置的选择的开始坐标
        /// </summary>
        public CPoint PStartBgPoint { get; set; }
        /// <summary>
        /// 调用Execute之前，需要重置的选择的结束坐标
        /// </summary>
        public CPoint PEndBgPoint { get; set; }

        /// <summary>
        /// 执行完成之后，需要显示选择部分开始坐标
        /// </summary>
        public CPoint PStartForExecuteAfterShowSelectPoint { get; set; }
        /// <summary>
        /// 执行完成之后，需要显示选择部分结束坐标
        /// </summary>
        public CPoint PEndForExecuteAfterShowSelectPoint { get; set; }

        #endregion

        #region 保存坐标

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wordPoint"></param>
        /// <param name="editPoint"></param>
        /// <param name="ssWord"></param>
        public void SetSurosrPoint(Point wordPoint, Point editPoint, SursorSelectWord ssWord) {
            this.pCousorPointForWord = new Point(wordPoint.X, wordPoint.Y);
            this.pCousorPointForEdit = new Point(editPoint.X, editPoint.Y);
            if (ssWord == null)
                return;
            this.pSursorSelectWord = new SursorSelectWord() {
                End = ssWord.End,
                LeftWidth = ssWord.LeftWidth,
                LeftWidthForWord = ssWord.LeftWidthForWord,
                LineIndex = ssWord.LineIndex,
                PWord = ssWord.PWord,
                PWordIndex = ssWord.PWordIndex
            };
        }

        public void SetSurosrPointLocal() {
            this.SetSurosrPoint(this.PParser.PCursor.CousorPointForWord, this.PParser.PCursor.CousorPointForEdit, this.PParser.PCursor.PSursorSelectWord);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        public void SetSurosrPointLocal(CPoint point) {
            int y = point.Y / FontContainer.FontHeight;
            int x = point.X - this.PParser.PIEdit.GetHorizontalScrollValue;
            if (x == this.PParser.PCursor.CousorPointForEdit.X)
                this.SetSurosrPointLocal();
            else {
                var ssWord = this.PParser.GetLineStringIndex(this.PParser.PLineString[y], point.X);
                this.pCousorPointForEdit = new Point(point.X - this.PParser.PIEdit.GetHorizontalScrollValue, point.Y);
                this.pCousorPointForWord = new Point(ssWord.LineIndex, y);
                this.pSursorSelectWord = ssWord;
            }
            //this.pCousorPointForWord = new Point(wordPoint.X, wordPoint.Y);
            //this.pCousorPointForEdit = new Point(editPoint.X, editPoint.Y);

        }

        public void SetSurosrPoint() {
            this.PActionOperation.SetSurosrPoint(this.PParser.PCursor.CousorPointForWord, this.PParser.PCursor.CousorPointForEdit, this.PParser.PCursor.PSursorSelectWord);
        }

        protected void SetSelectBg() {
            if (this.PParser.GetSelectPartPoint != null)
                this.SetSelectBg(this.PParser.GetSelectPartPoint[0], this.PParser.GetSelectPartPoint[1]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        public void SetSelectBg(CPoint startPoint, CPoint endPoint) {
            this.PActionOperation.SetSelectBgLocal(startPoint, endPoint);
        }


        public void SetSelectBgLocal(CPoint startPoint, CPoint endPoint) {
            if (startPoint != null)
                this.PStartBgPoint = new CPoint(startPoint.X, startPoint.Y, startPoint.LineWidth, startPoint.LineStringIndex);
            if (endPoint != null)
                this.PEndBgPoint = new CPoint(endPoint.X, endPoint.Y, endPoint.LineWidth, endPoint.LineStringIndex);
        }

        /// <summary>
        /// 设置选择的背景行宽度
        /// </summary>
        /// <param name="width"></param>
        public void SetSelectGgLineWidth(int width) {
            if (this.PActionOperation.PStartBgPoint != null)
                this.PActionOperation.PStartBgPoint.LineWidth = width;
        }

        //public void SetSelectBg() {
        //    if (this.PParser.GetSelectPartPoint != null) {
        //        var startPoint = this.PParser.GetSelectPartPoint[0];
        //        var endPoint = this.PParser.GetSelectPartPoint[1];
        //        this.PActionOperation.PStartBgPoint = new CPoint(startPoint.X, startPoint.Y, startPoint.LineWidth, startPoint.LineStringIndex);
        //        this.PActionOperation.PEndBgPoint = new CPoint(endPoint.X, endPoint.Y, endPoint.LineWidth, endPoint.LineStringIndex);
        //    }
        //}

        public void SetDrawBgLocal() {
            this.SetDrawBgLocal(this.PParser.GetSelectPartPoint);
        }

        public void SetDrawBgLocal(CPoint[] points) {
            if (points != null) {
                var startPoint = points[0];
                var endPoint = points[1];
                this.PStartForExecuteAfterShowSelectPoint = new CPoint(startPoint.X, startPoint.Y, startPoint.LineWidth, startPoint.LineStringIndex);
                this.PEndForExecuteAfterShowSelectPoint = new CPoint(endPoint.X, endPoint.Y, endPoint.LineWidth, endPoint.LineStringIndex);
            }
        }


        public void SetDrawBg() {
            if (this.PParser.GetSelectPartPoint != null) {
                var startPoint = this.PParser.GetSelectPartPoint[0];
                var endPoint = this.PParser.GetSelectPartPoint[1];
                this.PActionOperation.PStartForExecuteAfterShowSelectPoint = new CPoint(startPoint.X, startPoint.Y, startPoint.LineWidth, startPoint.LineStringIndex);
                this.PActionOperation.PEndForExecuteAfterShowSelectPoint = new CPoint(endPoint.X, endPoint.Y, endPoint.LineWidth, endPoint.LineStringIndex);
            }

        }

        public CPoint[] GetSelectBg() {
            if (this.PParser.GetSelectPartPoint != null) {
                var startPoint = this.PParser.GetSelectPartPoint[0];
                var endPoint = this.PParser.GetSelectPartPoint[1];
                return new CPoint[] { 
                    new CPoint(startPoint.X, startPoint.Y, startPoint.LineWidth, startPoint.LineStringIndex),
                    new CPoint(endPoint.X, endPoint.Y, endPoint.LineWidth, endPoint.LineStringIndex)
                };
            }
            return null;
        }

        #endregion

        #region 重置

        public virtual void RestBgPoint() {
            this.PParser.SetBgStartPoint(this.PStartBgPoint);
            this.PParser.SetBgEndPoint(this.PEndBgPoint);
        }

        protected void RestBgDrawPoint() {
            this.PParser.SetBgStartPoint(this.PStartForExecuteAfterShowSelectPoint);
            this.PParser.SetBgEndPoint(this.PEndForExecuteAfterShowSelectPoint);
        }

        protected void RestBgDrawPoint(CPoint[] points) {
            this.PParser.SetBgStartPoint(points[0]);
            this.PParser.SetBgEndPoint(points[1]);
        }

        public virtual void ResetCursorPoint() {
            //if (this is PuckerAction)
            //    return;
            this.PParser.PCursor.CousorPointForWord = new Point(this.pCousorPointForWord.X, this.pCousorPointForWord.Y);
            this.PParser.PCursor.CousorPointForEdit = new Point(this.pCousorPointForEdit.X, this.pCousorPointForEdit.Y);
            this.PParser.PCursor.XForLeft = this.pCousorPointForEdit.X - this.PParser.GetLeftSpace;
            if (this.pSursorSelectWord == null)
                return;
            this.PParser.PCursor.PSursorSelectWord = new SursorSelectWord() {
                End = this.pSursorSelectWord.End,
                LeftWidth = this.pSursorSelectWord.LeftWidth,
                LeftWidthForWord = this.pSursorSelectWord.LeftWidthForWord,
                LineIndex = this.pSursorSelectWord.LineIndex,
                PWord = this.pSursorSelectWord.PWord,
                PWordIndex = this.pSursorSelectWord.PWordIndex
            };
            //this.PParser.PCursor.SetPosition(
        }

        /// <summary>
        /// 重置光标和选择背景坐标
        /// Execute 方法执行之前执行
        /// </summary>
        public virtual void ResetPoint() {
            //this.SetSurosrPoint(this.PParser.PCursor.CousorPointForWord, this.PParser.PCursor.CousorPointForEdit, this.PParser.PCursor.PSursorSelectWord);
            //this.SetSelectBg();
            this.RestBgPoint();
            this.ResetCursorPoint();
        }

        #endregion

        #region

        /// <summary>
        /// 设置当前相反操作
        /// </summary>
        protected void SetOperationAction() {
            this.PActionOperation = this.GetOperationAciton();
            if (this.PActionOperation != null)
                this.PActionOperation.PIsUndoOrRedo = this.PIsUndoOrRedo;
        }


        /// <summary>
        /// 获取当前相反操作
        /// </summary>
        protected virtual BaseAction GetOperationAciton() {
            return null;
        }


        /// <summary>
        /// 获取一个相反的操作
        /// </summary>
        /// <returns></returns>
        public virtual BaseAction OppositeOperation() {
            return this.PActionOperation;
        }

        #endregion

        /// <summary>
        /// 执行
        /// </summary>
        public virtual void Execute() {
            //this.SetNowLNProperty();
            this.SetOperationAction();
            this.PParser.ClearCouple();
        }

        public bool PIsUndoOrRedo { get; set; }

        #region 行属性



        protected int PStartLineY = -1;

        /// <summary>
        /// 获取有效的删除行个数
        /// </summary>
        protected int GetDeleteLineCount {
            get {
                return Math.Max(0, this.PDeleteLineCount - 1);
            }
        }

        /// <summary>
        /// 更改行的增量值
        /// </summary>
        /// <param name="incr">增量</param>
        protected void ChangeIncrementLine(int incr) {
            this.ChangeIncrementLine(incr, this.PParser.PCursor.CousorPointForWord.Y);
        }

        /// <summary>
        /// 更改行的增量值
        /// </summary>
        /// <param name="incr">增量</param>
        /// <param name="startLineY">开始的行</param>
        protected void ChangeIncrementLine(int incr, int startLineY) {
            if (incr == 0)
                return;
            //startLineY++;
            //Parallel.Invoke(() => {
            //    //this.PParser.ClearStartEndPostion(this.PStartLineY == -1 ? startLineY - 1 : this.PStartLineY, incr);
            //    this.PParser.IncrementLineNum(incr);
            //},
            //() => { this.ChangeSelectPuckerY(startLineY, incr); }
            //);

            this.ChangeSelectPuckerY(startLineY, incr);
        }

        private void ChangeSelectPuckerY(int y, int incr) {
            if (this.PParser.PPucker.SelectPuckerStartY < 0)
                return;
            y = y + Math.Sign(incr) * Math.Abs(incr) * -1;
            if (this.PParser.PPucker.SelectPuckerEndY > y && this.PParser.PPucker.SelectPuckerStartY <= y) {
                this.PParser.PPucker.SelectPuckerEndY += incr;
                if (y == this.PParser.PPucker.SelectPuckerStartY
                     && (this.PParser.PCursor.CousorPointForWord.X != this.PParser.GetLineString.Length - 1)
                    ) {
                    this.PParser.PPucker.SelectPuckerStartY += incr;
                }
            } else if (this.PParser.PPucker.SelectPuckerEndY == y && this.PParser.GetLineString.Length > this.PParser.PCursor.CousorPointForWord.X + 1) {
                this.PParser.PPucker.SelectPuckerEndY += incr;
            }
        }

        /// <summary>
        /// 移除折叠剩余部分，同时将原来的折叠行的ID赋予新行。
        /// </summary>
        /// <param name="upLNPID"></param>
        /// <param name="ls"></param>
        protected void RemovePuckerLeavingOnly(Tuple<LineNodeProperty, int, bool> upLNPID, LineString ls) {
            if (upLNPID == null)
                return;

            var upLNP = upLNPID.Item1;
            if (upLNP != null && upLNP.IsFurl && ls.PLNProperty != null) {
                ls.PLNProperty.IsFurl = true;
                ls.ID = upLNPID.Item2;
                this.PParser.PPucker.RmovePuckerLeavingOnly(ls);
                return;
            }

            //#region 展开

            if (upLNPID.Item1 != null && upLNPID.Item1.IsFurl && !this.PParser.GetLineString.IsFurl()) {
                this.PParser.GetLineString.ID = upLNPID.Item2;
                this.PParser.GetLineString.IsCommentPucker = true;
                var puckerAction = new PuckerAction(this.PParser);
                puckerAction.SetPuckerIndexY(this.PParser.PCursor.CousorPointForWord.Y);
                puckerAction.ClickPucker();
                this.PParser.GetLineString.IsCommentPucker = false;
                this.PParser.AddAction(puckerAction);
                return;
            }

            //#endregion

            //if (upLNPID.Item3) {
            //    if (ls.IsStartRange()) {
            //        ls.ID = upLNPID.Item2;
            //        this.PParser.PPucker.RmovePuckerLeavingOnly(ls);
            //        ls.PLNProperty.IsFurl = true;
            //    }
            //}
        }

        /// <summary>
        /// 获取LineString 有效的文本
        /// </summary>
        /// <param name="ls">当前行</param>
        /// <returns></returns>
        protected string GetLineStringEffectualText(LineString ls) {
            if (ls.PLNProperty == null || !ls.PLNProperty.IsFurl)
                return ls.Text;
            return ls.Text + this.PParser.PPucker.PDictPuckerLeavings[ls.ID].Item1;
        }

        /// <summary>
        /// 获取LineString 有效的文本
        /// 折叠之后隐藏的部分字符串如“{之后”
        /// </summary>
        /// <returns></returns>
        public string GetLineStringEffectualText() {
            return this.GetLineStringEffectualText(this.PParser.GetLineString);
        }

        /// <summary>
        /// 重置当前行
        /// </summary>
        /// <param name="ls"></param>
        /// <param name="nowLineY"></param>
        /// <param name="text"></param>
        protected void SetResetLineString(LineString ls, string text = null) {
            if (text == null)
                text = ls.Text;

            this.PParser.PCharFontStyle.ClearProperty();
            var nl = this.PParser.PCharFontStyle.GetLineString(text/*, nowLineY*/);
            ls.PWord = nl.PWord;
            ls.Text = nl.Text;
            ls.PLNProperty = nl.PLNProperty;
            ls.PMoreLineStyles = nl.PMoreLineStyles;
            ls.IsCommentPucker = nl.IsCommentPucker;
        }


        protected void MergeLineStringChangeXY(LineString changeLine) {
            this.SetLineStringWidth(changeLine);
            this.PParser.PCursor.CousorPointForEdit.X = changeLine.Width + this.PParser.GetLeftSpace - this.PParser.PIEdit.GetHorizontalScrollValue;
            this.PParser.PCursor.CousorPointForWord.X = changeLine.Length - 1;
            this.PParser.PCursor.XForLeft = this.PParser.PCursor.CousorPointForEdit.X - this.PParser.GetLeftSpace;
        }

        private void SetLineStringWidth(LineString changeLine) {
            if (changeLine.Width == 0 && changeLine.PWord != null) {
                foreach (var w in changeLine.PWord)
                    changeLine.Width += w.Width;
            }
        }

        protected int GetLsWidth(LineString ls) {
            return this.PParser.GetLsWidth(ls);
        }

        #endregion

        #region 删除选择部分

        /// <summary>
        /// 删除的行个数
        /// </summary>
        protected int PDeleteLineCount;


        protected List<PuckerLineStringAndID> DeleteSeelctPuckerLineStringAndY { get; set; }

        /// <summary>
        /// 删除单行
        /// </summary>
        /// <param name="deleteLineCount"></param>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        private string DeleteSelectOneLine(CPoint startPoint, CPoint endPoint) {
            var sbStr = new StringBuilder();
            var indexY = startPoint.Y / FontContainer.FontHeight;
            var ls = this.PParser.PLineString[indexY];
            var lnpID = ls.GetLnpAndId();
            string delText = ls.Text.Substring(startPoint.LineStringIndex + 1, endPoint.LineStringIndex - startPoint.LineStringIndex);
            var puckerData = sbStr.AppendForPucker(ls, delText,
                                    endPoint.LineStringIndex, this.PParser.PPucker, indexY);



            string text = ls.Text;
            if (endPoint.LineStringIndex < ls.Length - 1)
                text = this.GetLineStringEffectualText(ls);
            text = text.Remove(startPoint.LineStringIndex + 1, endPoint.LineStringIndex - startPoint.LineStringIndex);

            this.SetResetLineString(ls, text);
            this.RemovePuckerLeavingOnly(lnpID, ls);

            this.PParser.PCursor.CousorPointForWord.X = startPoint.LineStringIndex;
            this.PParser.PCursor.SetPosition(startPoint.X, startPoint.Y, this.PParser.GetLeftSpace);
            this.PParser.PCursor.SetPosition();
            //this.SetLineLNP(line, _startY, lnps);
            //this.PParser.ClearSelect();
            SetStartLinePoint(puckerData);
            return sbStr.ToString();
        }

        private void SetStartLinePoint(ResultPuckerListData puckerData, bool isStart = true) {
            if (puckerData != null) {
                if (isStart)
                    this.PParser.GetSelectPartPoint[0].LineWidth += puckerData.LineLeavingWidth;

                var endPoint = this.PParser.GetSelectPartPoint[1];
                endPoint.X += puckerData.LineLeavingWidth;
                endPoint.LineWidth += puckerData.LineLeavingWidth;
                endPoint.LineStringIndex += Math.Max(0, puckerData.LineLeavingLength - 1);
                if (puckerData.PuckerCount > 0) {
                    endPoint.Y += puckerData.PuckerCount * FontContainer.FontHeight;
                    if (puckerData.LastChildLS != null) {
                        endPoint.X = this.PParser.GetLeftSpace + puckerData.LastChildLS.Width;
                        endPoint.LineWidth = puckerData.LastChildLS.Width;
                        endPoint.LineStringIndex = puckerData.LastChildLS.Length - 1;
                    }
                }
                if (puckerData.PuckerLineStringAndY != null && puckerData.PuckerLineStringAndY.Count > 0)
                    this.DeleteSeelctPuckerLineStringAndY = puckerData.PuckerLineStringAndY;
            }
        }

        /// <summary>
        /// 删除多行
        /// </summary>
        /// <param name="deleteLineCount"></param>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        private string DeleteSelectMoreLine(out int deleteLineCount, CPoint startPoint, CPoint endPoint) {
            deleteLineCount = 0;
            var sbStr = new StringBuilder();
            int i = startPoint.Y;
            int startIndex = startPoint.Y / FontContainer.FontHeight;
            int startY = startIndex;

            //int endY = 0;
            LineString ls = null;
            //var lpns = this.GetSelectLNProperty(startPoint, endPoint, out endY);
            //string delString = string.Empty;
            var puckerData = new ResultPuckerListData();
            puckerData.PuckerLineStringAndY = new List<PuckerLineStringAndID>();
            int puckerCount = 0;
            Tuple<LineNodeProperty, int, bool> lnpID = null;
            while (i <= endPoint.Y) {
                if (i == startPoint.Y) {
                    ls = this.PParser.PLineString[startIndex];
                    //sbStr.AppendLine(ls.Text.Substring(Math.Min(ls.Text.Length, startPoint.LineStringIndex + 1)));
                    var startPucker = sbStr.AppendForPucker(ls, ls.Text.Substring(Math.Min(ls.Text.Length, startPoint.LineStringIndex + 1)), this.PParser.PPucker);
                    this.PParser.PPucker.GetPuckerLsText(ls, sbStr, puckerData.PuckerLineStringAndY, startIndex, ref puckerCount);
                    ls.Text = ls.Text.Substring(0, Math.Min(ls.Text.Length, startPoint.LineStringIndex + 1));
                    startIndex++;
                    deleteLineCount++;
                    if (startPucker != null) {
                        startPoint.LineWidth += startPucker.Item2;
                        //startPoint.LineStringIndex += startPucker.Item1;
                    }
                } else if (i == endPoint.Y) {
                    var lendLs = this.PParser.PLineString[startIndex];
                    //sbStr.Append(lendLs.Text.Substring(0, Math.Min(lendLs.Text.Length, endPoint.LineStringIndex + 1)));
                    var endPucker = sbStr.AppendForPucker(lendLs,
                        lendLs.Text.Substring(0, Math.Min(lendLs.Text.Length, endPoint.LineStringIndex + 1)),
                        endPoint.LineStringIndex,
                        this.PParser.PPucker,
                        i / FontContainer.FontHeight
                        );
                    if (lendLs.Length > endPoint.LineStringIndex + 1) {
                        lnpID = lendLs.GetLnpAndId();
                        ls.Text += this.GetLineStringEffectualText(lendLs).Substring(endPoint.LineStringIndex + 1);
                    }

                    this.PParser.PLineString.RemoveAt(startIndex);
                    deleteLineCount++;
                    if (endPucker != null) {
                        puckerData.LineLeavingLength = endPucker.LineLeavingLength;
                        puckerData.LineLeavingWidth = endPucker.LineLeavingWidth;
                        puckerData.LastChildLS = endPucker.LastChildLS;
                        puckerCount += endPucker.PuckerCount;
                        if (endPucker.PuckerLineStringAndY.Count > 0)
                            puckerData.PuckerLineStringAndY.AddRange(endPucker.PuckerLineStringAndY);
                    }
                } else {
                    sbStr.AppendForPucker(this.PParser.PLineString[startIndex], this.PParser.PPucker);
                    this.PParser.PPucker.GetPuckerLsText(this.PParser.PLineString[startIndex], sbStr, puckerData.PuckerLineStringAndY,
                        i / FontContainer.FontHeight,
                        ref puckerCount);
                    this.PParser.PLineString.RemoveAt(startIndex);
                    deleteLineCount++;
                }
                i += FontContainer.FontHeight;
            }
            #region 重新解析当前行
            //this.PParser.PCharFontStyle.ClearProperty();
            //var nLine = this.PParser.PCharFontStyle.GetLineString(ls.Text/*, this.PParser.PCursor.CousorPointForWord.Y*/);
            //ls.PWord = nLine.PWord;
            //ls.Width = 0;
            //ls.PLNProperty = nLine.PLNProperty;
            //ls.PRecentlyUpPLN = nLine.PRecentlyUpPLN;

            this.SetResetLineString(ls);
            this.RemovePuckerLeavingOnly(lnpID, ls);

            #endregion
            puckerData.PuckerCount = puckerCount;
            this.SetStartLinePoint(puckerData, false);
            this.PParser.PCursor.CousorPointForWord.Y = startY;
            this.PParser.PCursor.CousorPointForWord.X = startPoint.LineStringIndex;
            this.PParser.PCursor.SetPosition(startPoint.X, startPoint.Y, this.PParser.GetLeftSpace);
            //if (this.PCursor.CousorPointForEdit.Y < this.PIEdit.GetVerticalScrollValue) {

            //}

            this.PParser.PIEdit.ChangeScollSize();
            this.PParser.PCursor.SetPosition();
            //this.SetMoreLineLnp(ls, lpns, startY, endY);
            //this.PParser.ClearSelect();
            return sbStr.ToString();
        }

        public string DeleteSelectPart(out int deleteLineCount) {
            deleteLineCount = 0;
            throw new Exception("需要实现");
        }

        /// <summary>
        /// 删除部分
        /// </summary>
        /// <param name="deleteLineCount">删除的行数</param>
        public string DeleteSelectPart(out int deleteLineCount, bool isClearSelect) {
            deleteLineCount = 0;
            this.PParser.PPucker.InitSelectPuckerStartEndY();
            var startPoint = this.PParser.GetSelectPartPoint[0];
            var endPoint = this.PParser.GetSelectPartPoint[1];
            if (startPoint.Y == endPoint.Y)
                return DeleteSelectOneLine(startPoint, endPoint);
            else
                return DeleteSelectMoreLine(out deleteLineCount, startPoint, endPoint);
        }


        protected void SetDeletePucker(CPoint[] selectBg) {
            if (this.DeleteSeelctPuckerLineStringAndY.IsNotNull()) {
                var puckerAction = new PuckerDeleteAction(this.PParser);
                if (selectBg != null) {
                    puckerAction.SetDrawBgLocal(selectBg);
                    puckerAction.SetSurosrPointLocal(selectBg[0]);
                }
                puckerAction.SetPucker(this.DeleteSeelctPuckerLineStringAndY, true);
                this.PParser.AddAction(puckerAction);
            }
        }

        protected void SetDrawBgClearSelectAndPucker(CPoint[] selectBgs) {
            this.SetDrawBg();
            this.PParser.ClearSelect();
            this.SetDeletePucker(selectBgs);
        }

        #endregion
    }
}
