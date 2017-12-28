using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.Edit.Entity;

namespace XZ.Edit.Actions {
    /// <summary>
    /// 注释
    /// </summary>
    public class CommentAction : BaseAction {
        public CommentAction(Parser paser)
            : base(paser) {
            this.PCommentStartWidth = -1;
            //this.PMoreStartIndexY = -1;
            //this.PMoreEndIndexY = -1;
        }

        public string PCommentStartStr { get; set; }

        public int PCommentStartWidth { get; set; }

        /// <summary>
        /// 外部调用，并且是PCancel = true。这个时候回记录取消的LienString 索引
        /// </summary>
        public bool IsExternal { get; set; }

        private HashSet<int> CommentYs { get; set; }

        private PuckerLineStringAndID[] PuckerArrayID { get; set; }

        /// <summary>
        /// 最小插入位置
        /// </summary>
        private int minInsertWordIndex = int.MaxValue;
        private int minInsertLineIndex;
        /// <summary>
        /// 是否是取消
        /// </summary>
        public bool PCancel { get; set; }

        public override void Execute() {
            base.Execute();
            this.SetDrawBg();
            var isChange = false;
            if (this.PCancel)
                isChange = this.Cancel();
            else
                isChange = this.Select();
            if (!isChange) {
                this.PIsAddUndo = false;
                return;
            }
            this.PParser.PIEdit.SetChangeText();
            this.SetSurosrPoint();
            this.RestBgDrawPoint();
            this.PParser.PCursor.SetPosition();
            this.PParser.PIEdit.Invalidate();

        }

        /// <summary>
        /// 取消
        /// </summary>
        private bool Cancel() {
            if (this.PParser.GetSelectPartPoint == null) {
                var ls = this.PParser.GetLineString;
                return this.CancelLine(ls, true);
            } else {
                HashSet<int> commentIDs = null;
                if (this.IsExternal)
                    commentIDs = new HashSet<int>();

                var startLs = this.PParser.PLineString[this.PParser.GetSelectPartPoint[0].Y / FontContainer.FontHeight];
                var startWidth = startLs.Width;
                var endY = this.PParser.GetSelectPartPoint[1].Y;
                for (var lineY = this.PParser.GetSelectPartPoint[0].Y; lineY <= endY; lineY += FontContainer.FontHeight) {
                    int indexY = lineY / FontContainer.FontHeight;
                    if (!this.CancelLine(this.PParser.PLineString[indexY], false) && commentIDs != null) {
                        commentIDs.Add(indexY);
                    }
                }

                if (this.PCommentStartWidth > 0) {
                    if (!string.IsNullOrWhiteSpace(startLs.Text))
                        this.PParser.GetSelectPartPoint[0].LineWidth -= this.PCommentStartWidth;

                    var lastLs = this.PParser.PLineString[this.PParser.GetSelectPartPoint[1].Y / FontContainer.FontHeight];
                    if (!string.IsNullOrWhiteSpace(lastLs.Text)) {
                        this.PParser.GetSelectPartPoint[1].LineWidth -= this.PCommentStartWidth;
                        this.PParser.GetSelectPartPoint[1].X -= this.PCommentStartWidth;
                        this.PParser.GetSelectPartPoint[1].LineStringIndex -= this.PCommentStartStr.Length;
                    }
                }
                if (PuckerArrayID != null) {
                    this.PParser.PPucker.PuckerMarker.Clear();
                    for (var i = PuckerArrayID.Length - 1; i > -1; i--) {
                        var pl = PuckerArrayID[i];
                        this.PParser.PPucker.ClickPuckerUnfurl(this.PParser.PLineString[pl.IndexY], pl.IndexY, PuckerArrayID.Length > 1);
                    }
                }
                this.SetSelectBg();
                if (commentIDs != null)
                    (this.PActionOperation as CommentAction).CommentYs = commentIDs;

            }
            return true;
        }

        private bool CancelLine(LineString ls, bool setCousor) {
            if (string.IsNullOrWhiteSpace(ls.Text))
                return false;
            var index = 0;
            foreach (var w in ls.PWord) {
                if (!(w.PEWordType == EWordType.Space || w.PEWordType == EWordType.Tab)) {
                    if (w.Text.StartsWith(this.PCommentStartStr)) {
                        ls.Text = ls.Text.Remove(index, this.PCommentStartStr.Length);
                        this.SetResetLineString(ls);
                        if (setCousor && this.PCommentStartWidth > -1) {
                            this.PParser.PCursor.CousorPointForWord.X -= this.PCommentStartStr.Length;
                            this.PParser.PCursor.CousorPointForEdit.X -= this.PCommentStartWidth;
                        }
                        return true;
                    }
                    break;
                }
                index += w.Length;
            }

            return false;
        }

        #region 注释

        /// <summary>
        /// 注释
        /// </summary>
        public bool Select() {
            if (this.PParser.GetSelectPartPoint == null || this.PParser.GetSelectPartPoint[0].Y == this.PParser.GetSelectPartPoint[1].Y) {
                var ls = this.PParser.GetLineString;
                return this.SelectLine(ls);
            } else {
                this.DeleteSeelctPuckerLineStringAndY = new List<PuckerLineStringAndID>();
                int indexY = this.PParser.GetSelectPartPoint[0].Y / FontContainer.FontHeight;
                var startIndexY = indexY;
                int puckerLength = 0;
                int width = this.PParser.GetLsWidth(this.PParser.PLineString[indexY]);
                var endY = this.PParser.GetSelectPartPoint[1].Y;
                for (var lineY = this.PParser.GetSelectPartPoint[0].Y; lineY <= endY; lineY += FontContainer.FontHeight) {
                    var ls = this.PParser.PLineString[lineY / FontContainer.FontHeight];
                    this.GetHidePuckerID(ref indexY, ref puckerLength, ls, this.DeleteSeelctPuckerLineStringAndY);
                    indexY++;
                }
                this.PuckerShowAndComment();
                this.CommentAllLine(startIndexY, indexY - startIndexY);
                if (this.PCommentStartWidth == -1)
                    return false;

                #region 设置选择坐标
                indexY--;
                var startLs = this.PParser.PLineString[startIndexY];
                var cpoint = new CPoint();
                cpoint.X = this.PParser.GetLeftSpace;
                cpoint.Y = startIndexY * FontContainer.FontHeight;
                cpoint.LineWidth = width + (!string.IsNullOrWhiteSpace(startLs.Text) ? this.PCommentStartWidth : 0);
                cpoint.LineStringIndex = -1;
                this.PParser.SetBgStartPoint(cpoint);
                var lastLS = this.PParser.PLineString[indexY];
                width = this.PParser.GetLsWidth(lastLS);
                cpoint = new CPoint();
                cpoint.X = this.PParser.GetLeftSpace + width;
                cpoint.Y = indexY * FontContainer.FontHeight;
                cpoint.LineWidth = width - (string.IsNullOrWhiteSpace(startLs.Text) ? this.PCommentStartWidth : 0);
                cpoint.LineStringIndex = lastLS.Length - 1;
                this.PParser.SetBgEndPoint(cpoint);
                this.SetSelectBg();
                #endregion


                #region 赋值
                this.SetPuckerArrayID();
                #endregion

            }

            return true;
        }


        /// <summary>
        /// 注释单行
        /// </summary>
        /// <param name="ls"></param>
        private bool SelectLine(LineString ls) {
            if (string.IsNullOrWhiteSpace(ls.Text))
                return false;

            if (ls.IsFurl()) {
                var width = this.PParser.GetLsWidth(ls);
                this.DeleteSeelctPuckerLineStringAndY = new List<PuckerLineStringAndID>();
                int indexY = this.PParser.PCursor.CousorPointForWord.Y;
                var startIndexY = indexY;
                int puckerLength = 0;
                var arrayPuckerHide = this.GetHidePuckerID(ref indexY, ref puckerLength, ls, this.DeleteSeelctPuckerLineStringAndY);

                this.PuckerShowAndComment();
                this.CommentAllLine(startIndexY, indexY - startIndexY);
                if (this.PCommentStartWidth == -1)
                    return false;

                #region 设置选择坐标
                var cpoint = new CPoint();
                cpoint.X = this.PParser.GetLeftSpace;
                cpoint.Y = startIndexY * FontContainer.FontHeight;
                cpoint.LineWidth = width + this.PCommentStartWidth;
                cpoint.LineStringIndex = -1;
                this.PParser.SetBgStartPoint(cpoint);
                cpoint = new CPoint();
                var lastLs = arrayPuckerHide.Last();
                width = GetLsWidth(lastLs);
                cpoint.X = this.PParser.GetLeftSpace + width;
                cpoint.Y = indexY * FontContainer.FontHeight;
                cpoint.LineWidth = width;
                cpoint.LineStringIndex = lastLs.Length - 1;
                this.PParser.SetBgEndPoint(cpoint);
                this.SetSelectBg();
                #endregion


                #region 赋值
                this.SetPuckerArrayID();
                #endregion

            } else {
                int leftIndex = 0;
                int lsWidth = ls.Width;
                int wordIndex = InsertText(ls, ref leftIndex);
                if (wordIndex == -1)
                    return false;
                if (wordIndex < this.minInsertWordIndex) {
                    this.minInsertWordIndex = wordIndex;
                    this.minInsertLineIndex = leftIndex;
                }
                this.CommentAllLine(this.PParser.PCursor.CousorPointForWord.Y, 1);

                var width = this.PCommentStartWidth;
                //var w = ls.PWord[wordIndex];
                //var width = CharCommand.GetCharWidth(this.PParser.PIEdit.GetGraphics, this.PCommentStartStr, w.PIncluedFont == null ? null : w.PIncluedFont.PFont);
                if (width > 0 && leftIndex < this.PParser.PCursor.CousorPointForWord.X) {
                    this.PParser.PCursor.CousorPointForWord.X += this.PCommentStartStr.Length;
                    this.PParser.PCursor.CousorPointForEdit.X += width;
                    //this.PCommentStartWidth = width;
                    //this.PParser.ClearSelect();
                    (this.PActionOperation as CommentAction).PCommentStartWidth = this.PCommentStartWidth;
                    lsWidth += width;
                    this.PParser.SetBgStartPoint(new CPoint(this.PParser.GetLeftSpace, this.PParser.PCursor.CousorPointForEdit.Y, lsWidth, -1));
                    this.PParser.SetBgEndPoint(new CPoint(this.PParser.GetLeftSpace + lsWidth, this.PParser.PCursor.CousorPointForEdit.Y, lsWidth, ls.Length - 1));
                }
            }

            return true;
        }


        /// <summary>
        /// 获取隐藏的Pucker所在行列表。
        /// </summary>
        /// <param name="indexY"></param>
        /// <param name="id"></param>
        /// <param name="listID"></param>
        /// <param name="isFrist"></param>
        /// <returns></returns>
        private LineString[] GetHidePuckerID(ref int indexY, ref int puckerLength, LineString ls, List<PuckerLineStringAndID> listID) {
            var isFurl = ls.IsFurl();
            if (isFurl)
                listID.Add(new PuckerLineStringAndID(indexY));

            var id = ls.ID;
            if (/*!isFurl && */!string.IsNullOrWhiteSpace(ls.Text))
                this.ResetLineString(ls, indexY);

            //if (!isFurl)
            //    return null;
            LineString[] outValue;
            if (this.PParser.PPucker.PDictPuckerList.TryGetValue(id, out outValue)) {
                foreach (var l in outValue) {
                    indexY++;
                    this.GetHidePuckerID(ref indexY, ref puckerLength, l, listID);
                }
            }
            return outValue;
        }

        /// <summary>
        /// 展开节点，同时注释
        /// </summary>
        private void PuckerShowAndComment() {
            if (this.DeleteSeelctPuckerLineStringAndY.IsNotNull()) {
                this.PuckerArrayID = new PuckerLineStringAndID[this.DeleteSeelctPuckerLineStringAndY.Count];
                this.DeleteSeelctPuckerLineStringAndY.CopyTo(this.PuckerArrayID);
                foreach (var pl in this.DeleteSeelctPuckerLineStringAndY) {
                    var fLs = this.PParser.PLineString[pl.IndexY];
                    this.PParser.PPucker.ClickPuckerUnfurl(fLs, pl.IndexY, this.DeleteSeelctPuckerLineStringAndY.Count > 1);
                    //this.ResetLineString(fLs, pl.IndexY);
                }
            }
        }

        /// <summary>
        /// 将PuckerArrayID 赋给相反操作
        /// </summary>
        private void SetPuckerArrayID() {
            var commentAction = this.PActionOperation as CommentAction;
            commentAction.PCommentStartWidth = this.PCommentStartWidth;
            //commentAction.PMoreStartIndexY = this.PMoreStartIndexY;
            //commentAction.PMoreEndIndexY = this.PMoreEndIndexY;
            commentAction.PuckerArrayID = this.PuckerArrayID;
        }

        /// <summary>
        /// 重置行
        /// </summary>
        /// <param name="ls"></param>
        /// <param name="isFrist"></param>
        private void ResetLineString(LineString ls, int indexY) {
            if (this.CommentYs != null && this.CommentYs.Contains(indexY))
                return;
            int leftIndex = 0;
            int lsWidth = ls.Width;
            int wordIndex = this.InsertText(ls, ref leftIndex);
            if (wordIndex < this.minInsertWordIndex) {
                this.minInsertWordIndex = wordIndex;
                this.minInsertLineIndex = leftIndex;
            }
            //this.SetResetLineString(ls);
            //if (this.PCommentStartWidth == -1) {
            //    var w = ls.PWord[wordIndex];
            //    this.PCommentStartWidth = CharCommand.GetCharWidth(this.PParser.PIEdit.GetGraphics, this.PCommentStartStr, w.PIncluedFont == null ? null : w.PIncluedFont.PFont);
            //}
        }

        private int InsertText(LineString ls, ref int leftIndex) {
            for (var i = 0; i < ls.PWord.Count; i++) {
                var w = ls.PWord[i];
                if (!(w.PEWordType == EWordType.Space || w.PEWordType == EWordType.Tab)) {
                    //ls.Text = ls.Text.Insert(leftIndex, this.PCommentStartStr);
                    return i;
                }
                leftIndex += w.Length;
            }
            return -1;
        }

        /// <summary>
        /// 注释所有的行
        /// </summary>
        /// <param name="startY"></param>
        /// <param name="count"></param>
        private void CommentAllLine(int startY, int count) {
            count = startY + count;
            for (; startY < count; startY++) {
                var ls = this.PParser.PLineString[startY];
                if (string.IsNullOrWhiteSpace(ls.Text) || (this.CommentYs != null && this.CommentYs.Contains(startY)))
                    continue;
                //ls.PWord.Insert(this.minInsertWordIndex,
                ls.Text = ls.Text.Insert(this.minInsertLineIndex, this.PCommentStartStr);
                this.SetResetLineString(ls);
                if (this.PCommentStartWidth == -1) {
                    var w = ls.PWord[this.minInsertWordIndex];
                    this.PCommentStartWidth = CharCommand.GetCharWidth(this.PParser.PIEdit.GetGraphics, this.PCommentStartStr, w.PIncluedFont == null ? null : w.PIncluedFont.PFont);
                }

            }
        }

        #endregion

        protected override BaseAction GetOperationAciton() {
            var action = new CommentAction(this.PParser);
            action.PCancel = !this.PCancel;
            action.PCommentStartStr = this.PCommentStartStr;
            action.IsExternal = this.IsExternal;
            return action;
        }


    }
}
