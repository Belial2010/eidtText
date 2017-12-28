using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Entity {
    public class LineNode {
        public LineNode(LineString ls) {
            this.PLineString = ls;
            //if (ls != null)
            //    this.PLineString.PNode = this;
        }

        /// <summary>
        /// 子节点个数
        /// </summary>
        public int ChildAllCount { get; set; }

        /// <summary>
        /// 行属性
        /// </summary>
        public LineString PLineString { get; set; }

        /// <summary>
        /// 父类节点
        /// </summary>
        public LineNode Father { get; set; }
        //public FatherNode Father { get; set; }

        /// <summary>
        /// 上一个节点
        /// </summary>
        internal LineNode UpNode { get; set; }

        /// <summary>
        /// 下一个节点
        /// </summary>
        internal LineNode NextNode { get; set; }

        /////// <summary>
        /////// 第一个节点
        /////// </summary>
        //public LineNode FirstNode { get; set; }

        ///// <summary>
        ///// 最后一个节点
        ///// </summary>
        //public LineNode LastNode { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public CollectionLineNode ChildNodes { get; set; }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="ls"></param>
        public void Add(LineString ls) {
            this.Add(new LineNode(ls));
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="node"></param>
        public void Add(LineNode node) {
            //this.ChangeChildCount(1);
            //node.Father = new FatherNode(this);
            //if (ChildNodes == null)
            //   this.ChildNodes = new CollectionLineNode();

            //this.ChildNodes.Add(node);

            this.Add(null, node);
        }

        public void ChangeChildCount(int number) {
            this.ChildAllCount += number;
            if (this.Father != null)
                this.Father.ChangeChildCount(number);
        }


        //public void Insert(int index, LineString ls) {
        //    this.Insert(index, new LineNode(ls));
        //}

        //public void Insert(int index, LineNode node) {
        //    if (this.ChildNodes == null)
        //        this.ChildNodes = new List<LineNode>();

        //    this.ChildNodes.Insert(index, node);
        //    node.Father = new FatherNode(this);
        //    if (!node.PLineString.IsStartRange())
        //        ChangeChildCount(1);
        //}

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="upNode">上一个节点</param>
        /// <param name="node">要添加的节点</param>
        public void Add(LineNode upNode, LineNode node) {
            //node.UpNode = upNode;
            //node.NextNode = upNode.NextNode;
            //upNode.NextNode = node;
            if (this.ChildNodes == null)
                this.ChildNodes = new CollectionLineNode();

            //this.ChildNodes.Add(node);
            node.Father = upNode == null ? this : upNode.Father;
            if (upNode == null || !node.PLineString.IsStartRange())
                ChangeChildCount(1);
            this.ChildNodes.Add(upNode, node);
        }

        /// <summary>
        /// 移除节点
        /// </summary>
        /// <param name="nextNode"></param>
        /// <param name="node"></param>
        public void Remove(LineNode node) {
            //if (this.ChildNodes != null)
            //    this.ChildNodes.Remove(node);
            if (!node.PLineString.IsStartRange())
                ChangeChildCount(-1);
            this.ChildNodes.Remove(node);
        }

        public void Remove(LineString ls) {
            //this.Remove(ls.PNode);
        }


        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="upLs"></param>
        /// <param name="addLs"></param>
        public void Add(LineString upLs, LineString addLs) {
            //this.Add(upLs == null ? null : upLs.PNode, new LineNode(addLs));
        }

        /// <summary>
        /// 插入到第一行
        /// </summary>
        /// <param name="node"></param>
        public void Insert(LineNode node) {
            this.ChildNodes.Insert(node);
        }

        public void Insert(LineString ls) {
            this.Insert(new LineNode(ls));
        }

        //public void AddChild()


        public override string ToString() {
            if (this.PLineString != null)
                return this.PLineString.Text;
            else
                return string.Empty;
        }

        public LineNode Create(LineString ls = null) {
            var node = new LineNode(ls);
            //node.ChildAllCount = this.ChildAllCount;
            node.Father = this.Father;
            node.ChildNodes = this.ChildNodes;
            return node;
        }

        //public void Add

    }

    /// <summary>
    /// 父类节点
    /// </summary>
    public class FatherNode {
        public FatherNode(LineNode node) {
            this.Node = node;
        }
        public LineNode Node { get; set; }
    }
}
