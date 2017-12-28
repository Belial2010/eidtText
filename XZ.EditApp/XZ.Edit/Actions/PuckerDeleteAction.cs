using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XZ.Edit.Entity;

namespace XZ.Edit.Actions {
    /// <summary>
    /// 折叠
    /// </summary>
    public class PuckerDeleteAction : BaseAction {

        public PuckerDeleteAction(Parser paser)
            : base(paser) {
        }

        //private bool isResultNone;
        public bool IsDeletePucker { get; set; }

        private List<PuckerLineStringAndID> puckerLineStringAndY = new List<PuckerLineStringAndID>();

        //public void SetPucker(int y, bool isInit = false) {
        //    //this.pLineString = ls;
        //    //this.pY = y;
        //    puckerLineStringAndY.Add(new PuckerLineStringAndID(y));
        //    if (isInit) {
        //        this.SetOperationAction();
        //        this.SetDrawBg();
        //        this.SetSurosrPoint();
        //        this.PParser.ClearCouple();
        //        //this.SetSurosrPoint();
        //    }
        //}
        /// <summary>
        /// 设置折叠，不能调用Execute 方法。只是将操作存放到撤销列表中，但该相反操作不放入重做列表中。
        /// </summary>
        /// <param name="puckerLineStringAndY"></param>
        /// <param name="isInit"></param>
        public void SetPucker(List<PuckerLineStringAndID> puckerLineStringAndY, bool isInit = false) {
            //this.pY = y;
            this.puckerLineStringAndY = puckerLineStringAndY;
            //if (isInit) {
            //this.SetOperationAction();
            var pucker = new PuckerDeleteAction(this.PParser);
            pucker.IsDeletePucker = true;
            pucker.puckerLineStringAndY = puckerLineStringAndY;
            pucker.SetDrawBgLocal(new CPoint[] { PStartForExecuteAfterShowSelectPoint, PEndForExecuteAfterShowSelectPoint });
            pucker.SetSurosrPointLocal(PStartForExecuteAfterShowSelectPoint);
            this.PActionOperation = pucker;
            this.SetSelectBg(PStartForExecuteAfterShowSelectPoint, PEndForExecuteAfterShowSelectPoint);
            this.PParser.ClearCouple();
        }

        public override void Execute() {
            //this.isResultNone = this.IsDeletePucker;
            base.Execute();
            //this.
            this.SetDrawBg();
            this.SetSelectBg(this.PStartForExecuteAfterShowSelectPoint, this.PEndForExecuteAfterShowSelectPoint);
            this.SetSurosrPoint();
            for (var i = this.puckerLineStringAndY.Count - 1; i > -1; i--) {
                var pl = this.puckerLineStringAndY[i];
                var ls = this.PParser.PLineString[pl.IndexY];
                if (pl.IsDeletePucker) {
                    //ls.PLNProperty.IsFurl = true;
                    pl.IsDeletePucker = false;
                } else
                    ls.PLNProperty.IsFurl = !ls.PLNProperty.IsFurl;
                this.PParser.PPucker.ClickPuckerUnfurl(ls, pl.IndexY, this.puckerLineStringAndY.Count > 1);
            }
            this.RestBgDrawPoint();
            this.SetSurosrPoint();
            this.PParser.PIEdit.Invalidate();
        }
        /// <summary>
        /// 回调，更改剪切选择的区域
        /// 因为折叠部分的区域在剪切的时候已经改变，但原始的选择区域保存在该对象中。
        /// 这个需要将该选择的区域替换原来的区域
        /// </summary>
        /// <param name="action"></param>
        private void CallBackRedoExecute(BaseAction action) {
            var pasterAction = this.PParser.PRedo.Last();
            if (pasterAction == null)
                return;
            var cutAction = pasterAction.OppositeOperation();
            if (cutAction == null)
                return;

            cutAction.SetSelectBgLocal(action.PStartBgPoint, action.PEndBgPoint);

        }

        protected override BaseAction GetOperationAciton() {
            if (this.IsDeletePucker) {
                var nAction = new NoneAction(this.PParser);
                nAction.RedoExecute = CallBackRedoExecute;
                return nAction;
            }

            var pucker = new PuckerDeleteAction(this.PParser);
            //pucker.IsDeleteCreate = this.IsDeleteCreate;
            pucker.SetPucker(this.puckerLineStringAndY);
            return pucker;

            //return new NoneAction(this.PParser);
        }
    }
}
