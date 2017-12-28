using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Entity {
    public class PuckerLineStringAndID {

        public PuckerLineStringAndID(int indexY) : this(false, indexY) { }

        //public PuckerLineStringAndID(LineString ls, int indexY) {
        //    //this.PLS = ls;
        //    this.IndexY = indexY;
        //}
        public PuckerLineStringAndID(bool lsDeletePucker, int indexY) {
            this.IndexY = indexY;
            this.IsDeletePucker = lsDeletePucker;
        }


        /// <summary>
        /// 行
        /// </summary>
        //public LineString PLS { get; set; }

        /// <summary>
        /// 所在的Y索引
        /// </summary>
        public int IndexY { get; set; }

        [Obsolete]
        public bool IsDeletePucker { get; set; }
    }
}
