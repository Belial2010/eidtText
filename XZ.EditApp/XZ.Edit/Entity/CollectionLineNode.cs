using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XZ.Edit.Entity {
    public class CollectionLineNode : IEnumerable {

        public LineNode PNode { get; set; }

        /// <summary>
        /// 第一个节点
        /// </summary>
        public LineNode FirstNode { get; set; }

        public int Count { get; set; }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="upNode"></param>
        /// <param name="addNode"></param>
        public void Add(LineNode upNode, LineNode addNode) {
            //addNode.Father.Node.ChangeChildCount(1);
            if (upNode == null) {
                if (this.PNode == null) {
                    this.FirstNode = addNode;
                    this.PNode = addNode;
                } else {
                    addNode.UpNode = this.PNode;
                    this.PNode.NextNode = addNode;
                    this.PNode = addNode;
                }
            } else {
                if (upNode.NextNode != null) {
                    addNode.NextNode = upNode.NextNode;
                    upNode.NextNode.UpNode = addNode;
                }
                upNode.NextNode = addNode;
                addNode.UpNode = upNode;
            }
        }

        /// <summary>
        /// 插入到第一行
        /// </summary>
        /// <param name="addNode"></param>
        public void Insert(LineNode addNode) {
            this.FirstNode = addNode;
            if (this.PNode != null) {
                addNode.NextNode = this.PNode;
                if (this.PNode.NextNode != null)
                    this.PNode.NextNode.UpNode = addNode;
            }
            this.PNode = addNode;
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="removeNode"></param>
        public void Remove(LineNode removeNode) {
            //removeNode.Father.Node.ChangeChildCount(-1);
            if (removeNode.UpNode == null) {
                if (removeNode.NextNode == null) {
                    this.PNode = null;
                    return;
                }
                removeNode.NextNode.UpNode = null;
                return;
            }
            if (removeNode.NextNode == null) {
                removeNode.UpNode.NextNode = null;
                return;
            }
            removeNode.UpNode.NextNode = removeNode.NextNode.UpNode;
        }

        public IEnumerator GetEnumerator() {

            yield return null;
        }
    }
}
