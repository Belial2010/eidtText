using System;
using System.Drawing;


namespace XZ.Edit.Entity {
    public class PuckerMarker {

        //public PuckerMarker(int positionY, bool isStart,bool isFurl,int id) {
        //    this.PositionY = positionY;
        //    this.IsStart = isStart;
        //    this.IsFurl = isFurl;
        //    this.ID = id;
        //}


        /// <summary>
        /// 索引
        /// </summary>
        public int IndexY { get; set; }

        /// <summary>
        /// Y轴位置
        /// </summary>
        public int PositionY { get; set; }

        /// <summary>
        /// 是否是开始
        /// </summary>
        public bool IsStart { get; set; }

        public bool IsFurl { get; set; }

        public int ID { get; set; }

        //public string Text { get; set; }

        //public override string ToString() {
        //    return this.Text;
        //}
    }
}
