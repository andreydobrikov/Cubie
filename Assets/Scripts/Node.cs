using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cubie.Game
{
	public class Node<T>
	{
		public T value { get; set; }
		public List<Node<T>> children { get; }
		public int y { get; set; }					//depth from the very top root of the tree
		public float x { get; set; }
		public Node<T> parent { get; set; }
		public float mod { get; set; }
		public float width { get; set; }
		public int height { get; set; }

		public Node(Node<T> parent)
		{
			this.parent = parent;
			children = new List<Node<T>>();
		}

		public bool IsLeaf()
		{
			return this.children.Count == 0;
		}

		public bool IsLeftMost()
		{
			if (parent == null)
				return true;

			return parent.children[0] == this;
		}

		public bool IsRightMost()
		{
			if (parent == null)
				return true;

			return parent.children[parent.children.Count - 1] == this;
		}

		public Node<T> GetPreviousSibling()
		{
			if (parent == null || this.IsLeftMost())
				return null;

			return parent.children[parent.children.IndexOf(this) - 1];
		}

		public Node<T> GetNextSibling()
		{
			if (parent == null || IsRightMost())
				return null;

			return parent.children[parent.children.IndexOf(this) + 1];
		}

		public Node<T> GetLeftMostSibling()
		{
			if (parent == null)
				return null;

			if (IsLeftMost())
				return this;

			return parent.children[0];
		}

		public Node<T> GetLeftMostChild()
		{
			if (children.Count == 0)
				return null;

			return children[0];
		}

		public Node<T> GetRightMostChild()
		{
			if (children.Count == 0)
				return null;

			return children[children.Count - 1];
		}
	}

	public class NodeTree<T>
	{
		public int maxChildren { get; private set; }
		private readonly System.Random rnd = new System.Random();
		public int treeDepth { get; private set; }
		public int minTargetNodes { get; private set; }
		public int maxTargetNodes { get; private set; }
		public int targetValue { get; private set; }

		public List<Node<T>> flatNodeList { get; private set; }

		public NodeTree(NodeTreeConfig config)
		{
			maxChildren = config.maxLeaves + 1;
			treeDepth = rnd.Next(config.minDepth, config.maxDepth + 1);
			minTargetNodes = config.minTargetNodes;
			maxTargetNodes = config.maxTargetNodes;
			flatNodeList = new List<Node<T>>();
		}

		public Node<T> CreateTree(Node<T> parent, Func<T> valueGenerator, int depth = 0)
		{
			if (depth > treeDepth)
				return null;

			var node = new Node<T>(parent);
			node.value = valueGenerator();
			node.y = depth;
			flatNodeList.Add(node);

			var childrenCount = rnd.Next(1, maxChildren);

			//let's at least have 1 leaf for root
			if (childrenCount == 0 && depth == 0)
			{
				childrenCount = 1;
			}

			int leafDepth = depth + 1;
			for (var i = 0; i < childrenCount; i++)
			{
				if (leafDepth < treeDepth)
				{
					node.children.Add(CreateTree(node, valueGenerator, leafDepth));
					node.children[i].x = i;
					node.children[i].mod = 0;
				}
			}

			return node;
		}
	}
}