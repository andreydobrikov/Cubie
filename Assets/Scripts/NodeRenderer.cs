using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cubie.Game
{
	public class NodeRenderer : MonoBehaviour
	{
		[SerializeField] private GameObject nodePrefab;
		[SerializeField] private Transform treeRoot;
		[SerializeField] private Material lineMaterial;

		private Dictionary<NodeView, object> viewToNode = new Dictionary<NodeView, object>();

		public Action<int> OnValueAdded;

		public void ClearTree()
		{
			foreach (Transform child in treeRoot)
			{
				Destroy(child.gameObject);
			}

			viewToNode.Clear();
		}

		public void RenderTree<T>(NodeTree<T> nodeTree, Node<T> root)
		{
			if (root == null)
			{
				Debug.LogError("Root of the tree is null, please create a correct tree node");
				return;
			}

			//CalculateInitialPosition does the tree post traversal and figures out how to center parents over the children
			//as well as any conflicts that can arise when siblings of different parents collide
			NodeTreeUtils.CalculateInitialPosition(root);

			//Do a second triversal of the tree and add all mods to shift the tree node positions
			NodeTreeUtils.CalculateFinalPositions(root, 0);

			//Traverse the children recursively again to render the actual nodes at x, y positions
			RenderRoot(root, treeRoot, nodeTree);
		}

		public void SetParticlesActive(bool active)
		{
			foreach (NodeView view in viewToNode.Keys)
			{
				view.SetParticlesActive(active);
			}
		}

		public void RenderRoot<T>(Node<T> root, Transform parent, NodeTree<T> nodeTree)
		{
			GameObject gameObject = Instantiate(nodePrefab, parent);
			NodeView view = gameObject.GetComponent<NodeView>();
			view.RenderNode(root.value, root.x, root.y);
			view.OnViewClick = OnViewClick;
			viewToNode.Add(view, root);
			RenderLeaves(root, gameObject.transform, nodeTree);
		}

		public void RenderLeaves<T>(Node<T> root, Transform parent, NodeTree<T> nodeTree)
		{
			int count = root.children.Count;
			foreach (Node<T> child in root.children)
			{
				GameObject gameObject = Instantiate(nodePrefab, parent);
				gameObject.transform.localPosition = new Vector3(child.x - root.x, -1);
				NodeView view = gameObject.GetComponent<NodeView>();
				view.RenderNode(child.value, child.x, child.y);
				view.OnViewClick = OnViewClick;
				viewToNode.Add(view, child);

				//render line between parent and child
				DrawLine(parent.position, gameObject.transform.position, Color.grey, parent);

				RenderLeaves(child, gameObject.transform, nodeTree);
			}
		}

		private void OnViewClick(NodeView view)
		{
			object value;

			if (viewToNode.TryGetValue(view, out value) && value is Node<int>)
			{
				Node<int> val = value as Node<int>;
				int intValue = view.nodeSelected ? val.value : -val.value;
				OnValueAdded?.Invoke(intValue);
			}
		}

		private void DrawLine(Vector3 start, Vector3 end, Color color, Transform parent)
		{
			GameObject myLine = new GameObject();
			myLine.name = "Line" + start + end;
			myLine.transform.position = start;
			myLine.AddComponent<LineRenderer>();
			LineRenderer lr = myLine.GetComponent<LineRenderer>();
			lr.material = new Material(lineMaterial);
			lr.startColor = color;
			lr.endColor = color;
			lr.startWidth = 0.025f;
			lr.endWidth = 0.025f;
			lr.SetPosition(0, start);
			lr.SetPosition(1, end);

			myLine.transform.parent = parent; 
		}
	}
}
