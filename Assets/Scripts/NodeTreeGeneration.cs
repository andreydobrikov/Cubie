using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cubie.Game
{
	public class NodeTreeGeneration : MonoBehaviour
	{
		[SerializeField] private NodeTreeConfig treeConfig;
		[SerializeField] private NodeRenderer nodeRenderer;

		private Node<int> root;
		private NodeTree<int> nodeTree;
		private Func<int> valueGenerator => () => new System.Random().Next(treeConfig.maxNumber);

		public int currentValue { get; private set; }

		public Action<int> OnTotalValueChange;

		private void Start()
		{
			nodeRenderer.OnValueAdded = OnValueAdded;
		}

		private void OnDestroy()
		{
			nodeRenderer.OnValueAdded = null;
		}

		private void OnValueAdded(int value)
		{
			currentValue += value;
			OnTotalValueChange?.Invoke(currentValue);
		}

		public void SetAttempt(bool success)
		{
			nodeRenderer.SetParticlesActive(success);
		}

		public void GenerateTree()
		{
			if (treeConfig == null)
			{
				Debug.LogError("NodeTreeGeneration does not have NodeTreeConfig linked, add NodeTreeConfig in the inspector");
				return;
			}

			nodeTree = new NodeTree<int>(treeConfig);
			System.Random rand = new System.Random();
			root = nodeTree.CreateTree(null, valueGenerator);

			nodeRenderer.ClearTree();
			nodeRenderer.RenderTree(nodeTree, root);
		}

		public int FindTarget()
		{
			int minValue = Math.Min(nodeTree.minTargetNodes, nodeTree.flatNodeList.Count);
			int maxValue = Math.Min(nodeTree.maxTargetNodes, nodeTree.flatNodeList.Count) + 1;

			System.Random rand = new System.Random();
			int numNodes = rand.Next(minValue, maxValue);
			int target = 0;

			//pick the values from flat list
			List<Node<int>> nodes = nodeTree.flatNodeList.ToList();
			while (numNodes > 0)
			{
				int i = rand.Next(0, nodes.Count);
				target += nodes[i].value;
				nodes.RemoveAt(i);
				numNodes--;
			}

			return target;
		}
	}
}
