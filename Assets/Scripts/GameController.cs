using UnityEngine;

namespace Cubie.Game
{
	public class GameController : MonoBehaviour
	{
		[SerializeField] private UIController uiController;
		[SerializeField] private NodeTreeGeneration treeGeneration;
		[SerializeField] private AudioSource successAudioSource;

		private int target = 0;
		private int currentValue = 0;

		private void Start()
		{
			uiController.OnSubmitAction = SubmitAnswer;
			uiController.OnResetAction = ResetTree;
			treeGeneration.OnTotalValueChange = SetCurrentValue;

			ResetTree();
		}

		private void OnDestroy()
		{
			uiController.OnSubmitAction = null;
			uiController.OnResetAction = null;
			treeGeneration.OnTotalValueChange = null;
		}

		private void SubmitAnswer()
		{
			Debug.Log("Submitted value: " + currentValue);

			bool success = currentValue == target;

			uiController.SetAttempt(success);
			treeGeneration.SetAttempt(success);

			if (success)
			{
				successAudioSource.Play();
			}
		}

		private void ResetTree()
		{
			target = 0;
			currentValue = 0;
			treeGeneration.GenerateTree();
			FindTarget();
		}

		private void FindTarget()
		{
			target = treeGeneration.FindTarget();
			uiController.SetTarget(target);
		}

		private void SetCurrentValue(int value)
		{
			currentValue = value;
			Debug.Log("value: " + value);
		}
	}
}
