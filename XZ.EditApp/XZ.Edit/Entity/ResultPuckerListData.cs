using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Entity {
    public class ResultPuckerListData {
        /// <summary>
        /// 折叠行剩余宽度 
        /// </summary>
        public int LineLeavingWidth { get; set; }

        /// <summary>
        /// 折叠行剩余长度
        /// </summary>
        public int LineLeavingLength { get; set; }

        /// <summary>
        /// 折叠行已经所在的索引
        /// </summary>
        public List<PuckerLineStringAndID> PuckerLineStringAndY { get; set; }

        /// <summary>
        /// 折叠的行数
        /// </summary>
        public int PuckerCount { get; set; }

        public LineString LastChildLS { get; set; }
    }
}
