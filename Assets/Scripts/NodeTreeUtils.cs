using System;
using System.Collections.Generic;
using System.Linq;

namespace Cubie.Game
{
	public class NodeTreeUtils
	{
		private static int nodeSize = 1;
		private static float siblingDistance = 0f;
		private static float treeDistance = 0f;

		public static void CalculateInitialPosition<T>(Node<T> node)
		{
			foreach (var child in node.children)
			{
				CalculateInitialPosition(child);
			}

			// if no children
			if (node.IsLeaf())
			{
				// if there is a previous sibling in this set, set X to prevous sibling + designated distance
				if (!node.IsLeftMost())
				{
					node.x = node.GetPreviousSibling().x + nodeSize + siblingDistance;
				}
				else
				{
					node.x = 0;
				}
			}
			// if there is only one child
			else if (node.children.Count == 1)
			{
				// if this is the first node in a set, set it's x value equal to it's child's x value
				if (node.IsLeftMost())
				{
					node.x = node.children[0].x;
				}
				else
				{
					node.x = node.GetPreviousSibling().x + nodeSize + siblingDistance;
					node.mod = node.x - node.children[0].x;
				}
			}
			else
			{
				var leftChild = node.GetLeftMostChild();
				var rightChild = node.GetRightMostChild();
				var mid = (leftChild.x + rightChild.x) / 2;

				if (node.IsLeftMost())
				{
					node.x = mid;
				}
				else
				{
					node.x = node.GetPreviousSibling().x + nodeSize + siblingDistance;
					node.mod = node.x - mid;
				}
			}

			if (node.children.Count > 0 && !node.IsLeftMost())
			{
				// Since subtrees can overlap, check for conflicts and shift tree right if needed
				CheckForConflicts(node);
			}
		}

		private static void CheckForConflicts<T>(Node<T> node)
		{
			var minDistance = treeDistance + nodeSize;
			var shiftValue = 0f;

			var nodeContour = new Dictionary<int, float>();
			GetLeftContour(node, 0, ref nodeContour);

			var sibling = node.GetLeftMostSibling();
			while (sibling != null && sibling != node)
			{
				var siblingContour = new Dictionary<int, float>();
				GetRightContour(sibling, 0, ref siblingContour);

				for (int level = node.y + 1; level <= Math.Min(siblingContour.Keys.Max(), nodeContour.Keys.Max()); level++)
				{
					var distance = nodeContour[level] - siblingContour[level];
					if (distance + shiftValue < minDistance)
					{
						shiftValue = minDistance - distance;
					}
				}

				if (shiftValue > 0)
				{
					node.x += shiftValue;
					node.mod += shiftValue;

					CenterNodesBetween(node, sibling);

					shiftValue = 0;
				}

				sibling = sibling.GetNextSibling();
			}
		}

		private static void CenterNodesBetween<T>(Node<T> leftNode, Node<T> rightNode)
		{
			var leftIndex = leftNode.parent.children.IndexOf(rightNode);
			var rightIndex = leftNode.parent.children.IndexOf(leftNode);

			var numNodesBetween = (rightIndex - leftIndex) - 1;

			if (numNodesBetween > 0)
			{
				var distanceBetweenNodes = (leftNode.x - rightNode.x) / (numNodesBetween + 1);

				int count = 1;
				for (int i = leftIndex + 1; i < rightIndex; i++)
				{
					var middleNode = leftNode.parent.children[i];

					var desiredX = rightNode.x + (distanceBetweenNodes * count);
					var offset = desiredX - middleNode.x;
					middleNode.x += offset;
					middleNode.mod += offset;

					count++;
				}

				CheckForConflicts(leftNode);
			}
		}

		private static void GetLeftContour<T>(Node<T> node, float modSum, ref Dictionary<int, float> values)
		{
			if (!values.ContainsKey(node.y))
			{
				values.Add(node.y, node.x + modSum);
			}
			else
			{
				values[node.y] = Math.Min(values[node.y], node.x + modSum);
			}

			modSum += node.mod;
			foreach (var child in node.children)
			{
				GetLeftContour(child, modSum, ref values);
			}
		}

		private static void GetRightContour<T>(Node<T> node, float modSum, ref Dictionary<int, float> values)
		{
			if (!values.ContainsKey(node.y))
			{
				values.Add(node.y, node.x + modSum);
			}
			else
			{
				values[node.y] = Math.Max(values[node.y], node.x + modSum);
			}

			modSum += node.mod;
			foreach (var child in node.children)
			{
				GetRightContour(child, modSum, ref values);
			}
		}

		public static void CalculateFinalPositions<T>(Node<T> node, float modSum)
		{
			node.x += modSum;
			modSum += node.mod;

			foreach (var child in node.children)
			{
				CalculateFinalPositions(child, modSum);
			}

			if (node.children.Count == 0)
			{
				node.width = node.x;
				node.height = node.y;
			}
			else
			{
				node.width = node.children.OrderByDescending(p => p.width).First().width;
				node.height = node.children.OrderByDescending(p => p.height).First().height;
			}
		}
	}
}
