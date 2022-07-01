using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cubie.Game
{
	public class UIController : MonoBehaviour
	{
		[SerializeField] private Button submitButton;
		[SerializeField] private Button resetButton;
		[SerializeField] private TextMeshProUGUI targetText;
		[SerializeField] private GameObject success;

		public Action OnSubmitAction;
		public Action OnResetAction;

		private void Start()
		{
			submitButton.onClick.AddListener(OnSubmit);
			resetButton.onClick.AddListener(OnReset);
		}

		private void OnDestroy()
		{
			submitButton.onClick.RemoveListener(OnSubmit);
			resetButton.onClick.RemoveListener(OnReset);
		}

		private void OnSubmit()
		{
			OnSubmitAction?.Invoke();
		}

		private void OnReset()
		{
			success.SetActive(false);
			OnResetAction?.Invoke();
		}

		public void SetTarget(int target)
		{
			targetText.text = string.Format("Target: {0}", target);
		}

		public void SetAttempt(bool successValue)
		{
			success.SetActive(successValue);
		}
	}
}
