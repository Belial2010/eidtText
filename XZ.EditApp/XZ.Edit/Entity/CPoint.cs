using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Entity {

     

    /// <summary>
    /// 坐标
    /// </summary>
    public class CPoint {

        public CPoint() { }

        public CPoint(int x, int y)
            : this(x, y, 0, 0) {
        }

        public CPoint(int x, int y, int lingWidth)
            : this(x, y, lingWidth, 0) {
        }

        public CPoint(int x, int y, int lingWidth, int lsIndex) {
            this.X = x;
            this.Y = y;
            this.LineWidth = lingWidth;
            this.LineStringIndex = lsIndex;

        }

        public int X { get; set; }
        public int Y { get; set; }
        public int LineWidth { get; set; }
        public int LineStringIndex { get; set; }

        public CPoint Create() {
            return new CPoint(X, Y, LineWidth, LineStringIndex);
        }

        public override string ToString() {
            return string.Format("{0},{1},{2},{3}", this.X, this.Y, this.LineWidth, this.LineStringIndex);
        }

        /// <summary>
        /// 比较大小 -1小于，0相当，1大于比较的类型
        /// 只比较X，Y二个属性
        /// </summary>
        /// <param name="pint"></param>
        /// <returns></returns>
        public int CompareTo(CPoint point) {
            if (point.X == this.X && point.Y == this.Y)
                return 0;
            if (this.Y < point.Y || (this.Y == point.Y && this.X < point.X))
                return -1;
            else
                return 1;
        }

    }
}
