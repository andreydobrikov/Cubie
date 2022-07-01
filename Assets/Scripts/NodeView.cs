using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cubie.Game
{
	public class NodeView : MonoBehaviour, IPointerClickHandler
	{
		[SerializeField] private BoxCollider boxCollider;
		[SerializeField] private TextMeshPro valueText;
		[SerializeField] private TextMeshPro debugNodeText;
		[SerializeField] private MeshRenderer mr;
		[SerializeField] private ParticleSystem particles;

		public Action<NodeView> OnViewClick;

		public bool nodeSelected { get; private set; }

		public void RenderNode<T>(T value, float x, int y)
		{
			valueText.text = value.ToString();
			debugNodeText.text = string.Format("x: {0}, y: {1}", x, y);
			nodeSelected = false;
		}

		//Detect if a click occurs
		public void OnPointerClick(PointerEventData pointerEventData)
		{
			nodeSelected = !nodeSelected;
			mr.material.color = nodeSelected ? Color.green : Color.red;

			OnViewClick?.Invoke(this);
		}

		public void SetParticlesActive(bool active)
		{
			if (active)
			{
				particles.Play();
			}
			else
			{
				particles.Stop();
				particles.Clear();
			}
		}
	}
}
